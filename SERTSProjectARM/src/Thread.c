#include "cmsis_os.h"  // CMSIS RTOS header file
#include "Board_LED.h"
#include "UART_driver.h"
#include "stdint.h"                     // data type definitions
#include "stdio.h"                      // file I/O functions
#include "rl_usb.h"                     // Keil.MDK-Pro::USB:CORE
#include "rl_fs.h"                      // Keil.MDK-Pro::File System:CORE
#include "stm32f4xx_hal.h"
#include "stm32f4_discovery.h"
#include "stm32f4_discovery_audio.h"
#include <stdio.h>

#define LED_Green   0
#define LED_Orange  1
#define LED_Red     2
#define LED_Blue    3

//Global constants
//make sure sample rate is same in song
#define NUM_CHAN	2 // number of audio channels
#define NUM_POINTS 1024 // number of points per channel
#define BUF_LEN NUM_CHAN*NUM_POINTS // length of the audio buffer
/* buffer used for audio play */
int16_t Audio_Buffer1[BUF_LEN];
//buffer used for audio data
int16_t Audio_Buffer2[BUF_LEN];

char receive_file[150];

// Receive characters from the VB GUI
#define StopFile_char "1"
#define PlayFile_char "2"
#define PauseFile_char "3"

// Receive characters from the VB GUI
#define ShowFiles_char "4"
#define StartFileList_char "a"
#define EndFileList_char "6"
#define SendFileName_char "7"
#define RestartSong_Char "8"

////////////////////////////////////////////////
// State Machine definitions
enum state{
    NoState,
    Stop,
    Play,
    Pause,
    List
};

enum Triggers{
    StopFile,
    PlayFile,
    PauseFile,
    ListFiles,
    SendComplete
};

enum buffer_Type {
	Buffer1,
	Buffer2
};

// WAVE file header format
typedef struct WAVHEADER {
	unsigned char riff[4];						// RIFF string
	uint32_t overall_size;				// overall size of file in bytes
	unsigned char wave[4];						// WAVE string
	unsigned char fmt_chunk_marker[4];		// fmt string with trailing null char
	uint32_t length_of_fmt;					// length of the format data
	uint16_t format_type;					// format type. 1-PCM, 3- IEEE float, 6 - 8bit A law, 7 - 8bit mu law
	uint16_t channels;						// no.of channels
	uint32_t sample_rate;					// sampling rate (blocks per second)
	uint32_t byterate;						// SampleRate * NumChannels * BitsPerSample/8
	uint16_t block_align;					// NumChannels * BitsPerSample/8
	uint16_t bits_per_sample;				// bits per sample, 8- 8bits, 16- 16 bits etc
	unsigned char data_chunk_header [4];		// DATA string or FLLR string
	uint32_t data_size;						// NumSamples * NumChannels * BitsPerSample/8 - size of the next chunk that will be read
} WAVHEADER;

//////////////////////////////////////////////////////////
void Thread_1 (void const *argument);             // thread function 1
osThreadId tid_Thread_1;                      // thread id 1
osThreadDef (Thread_1, osPriorityNormal, 1, 0);      // thread object 1

//semaphore
osSemaphoreId (SEMO_id);
osSemaphoreDef (SEMO);

// message queue
osMessageQId message_Queue; // message queue for commands to Thread
osMessageQDef (Queue, 1, uint32_t); // message queue object

void Control (void const *argument); // thread function
osThreadId tid_Control; // thread id
osThreadDef (Control, osPriorityNormal, 1, 0); // thread object

// UART receive thread
void Rx_Command (void const *argument);  // thread function
osThreadId tid_RX_Command;  // thread id
osThreadDef (Rx_Command, osPriorityNormal, 1, 0); // thread object

// Command queue from Rx_Command to Controller
osMessageQId mid_CMDQueue; // message queue for commands to Thread
osMessageQDef (CMDQueue, 1, uint32_t); // message queue object

// Command queue from FSQueue
osMessageQId Command_FSQueue; //message queue for control to FS
osMessageQDef (FSQueue, 1, unit32_t); //message queue object

void Process_Event(uint16_t event){
    static uint16_t Current_State = NoState; // Current state of the SM
    switch(Current_State){
    case NoState:
        // Next State
        Current_State = Stop;
        // Exit actions
        // Transition actions
        // State1 entry actions
        LED_On(LED_Red);

        break;
    case Stop: // STOP
        if(event == PlayFile){
            Current_State = Play;
            // Exit actions
            LED_Off(LED_Red);
            // Transition actions
            osMessagePut(Command_FSQueue, 2, osWaitForever);
            // State2 entry actions
            LED_On(LED_Green);
        }
        if(event == ListFiles){
            Current_State = List;
            //Exit Actions
            LED_Off(LED_Red);
            //Transition actions
            osMessagePut(Command_FSQueue, 4, osWaitForever);
            //State entry actions
            LED_On(LED_Blue);
        }
        break;
    case List:
    	if(event == SendComplete){
    		Current_State = Stop;
    		// Exit actions
    		LED_Off(LED_Blue);
    		//transition actions
    		//next state entry actions
    		LED_On(LED_Red);
    	}
    	break;
    case Play: // PLAY
        if(event == PauseFile){
            Current_State = Pause;
            // Exit actions
            LED_Off(LED_Green);
            // Transition actions
            osMessagePut(Command_FSQueue, 3, osWaitForever);
            // State1 entry actions
            LED_On(LED_Orange);
        }
        if(event == StopFile){
            Current_State = Stop;
            // Exit actions
            LED_Off(LED_Green);
            // Transition actions
            osMessagePut(Command_FSQueue, 1, osWaitForever);
            UART_send("b\n", 2);
            // State3 entry actions
            LED_On(LED_Red);
        }
        break;
    case Pause: // PAUSE
        if(event == PlayFile){
            Current_State = Play;
            // Exit actions
            LED_Off(LED_Orange);
            // Transition actions
            osMessagePut(Command_FSQueue, 8, osWaitForever);
            // State2 entry actions
            LED_On(LED_Green);
        }
        if(event == StopFile){
            Current_State = Stop;
            // Exit actions
            LED_Off(LED_Orange);
            // Transition actions
            // State2 entry actions
            LED_On(LED_Red);
        }
        break;
    default:
        break;
    } // end case(Current_State)
} // Process_Event

void Init_Thread (void) {
    LED_Initialize(); // Initialize the LEDs
    UART_Init(); // Initialize the UART

    //initialize queue
    SEMO_id = osSemaphoreCreate(osSemaphore(SEMO), 0);

    //create message queue
    message_Queue = osMessageCreate (osMessageQ(Queue), NULL);  // create msg queue
    if (!message_Queue)return; // Message Queue object not created, handle failure

    // Create queues
    mid_CMDQueue = osMessageCreate (osMessageQ(CMDQueue), NULL);  // create msg queue
    if (!mid_CMDQueue)return; // Message Queue object not created, handle failure

    Command_FSQueue = osMessageCreate (osMessageQ(FSQueue), NULL);
    if (!Command_FSQueue)return;

    // Create threads
    tid_RX_Command = osThreadCreate (osThread(Rx_Command), NULL);
    if (!tid_RX_Command) return;

    tid_Control = osThreadCreate (osThread(Control), NULL);
    if (!tid_Control) return;

    tid_Thread_1 = osThreadCreate (osThread(Thread_1), NULL);
    if(!tid_Thread_1) return;
}

// pointer to file type for files on USB device
FILE *f;

//Thread 1: send songs to disco board and GUI
void Thread_1 (void const *argument) {
	fsFileInfo info;
	char *StartFileList_msg = "a\n";
	char *EndFileList_msg = "6\n";

	usbStatus ustatus; // USB driver status variable
	uint8_t drivenum = 0; // Using U0: drive number
	char *drive_name = "U0:"; // USB drive name
	fsStatus fstatus; // file system status variable

	WAVHEADER header;
	size_t rd;
	uint32_t i;
	static uint8_t rtrn = 0;
	uint32_t rdnum = 1; // read buffer number
	int r = 10;

	int16_t buff_Num = Buffer1; // variable to keep track of buffer

	ustatus = USBH_Initialize (drivenum); // initialize the USB Host
	if (ustatus == usbOK){
        // loop until the device is OK, may be delay from Initialize
        ustatus = USBH_Device_GetStatus (drivenum); // get the status of the USB device
        while(ustatus != usbOK){
            ustatus = USBH_Device_GetStatus (drivenum); // get the status of the USB device
        }
        // initialize the drive
        fstatus = finit (drive_name);
        if (fstatus != fsOK){
            // handle the error, finit didn't work
        } // end if
        // Mount the drive
        fstatus = fmount (drive_name);
        if (fstatus != fsOK){
            // handle the error, fmount didn't work
        } // end if
	}

	// initialize the audio output
	rtrn = BSP_AUDIO_OUT_Init(OUTPUT_DEVICE_AUTO, 0x46, 44100);
	if (rtrn != AUDIO_OK) return;
    while(1){
        //get a message and wait forever
        osEvent doSomething;
        doSomething = osMessageGet (Command_FSQueue, osWaitForever);
        uint16_t differentCases = doSomething.value.v;

        switch(differentCases) {
            //handle show files
            case(4):

                UART_send(StartFileList_msg,2); // Send start string

                info.fileID = 0;
                int Q = 0;

                while (ffind ("U0:*.*", &info) == fsOK) {
                    Q = strlen (info.name);
                    UART_send(info.name, Q);
                    UART_send("\n", 1);

                }
                UART_send(EndFileList_msg,2); // Send start string
                osMessagePut (mid_CMDQueue, SendComplete, 1);

                break;

            //handle play files
            case(2):
                f = fopen (receive_file,"r");// open a file on the USB device
                if (f != NULL) {
                    fread((void *)&header, sizeof(header), 1, f);
                }
                //populate buff1
                r =	(fread (Audio_Buffer1, sizeof(int16_t), BUF_LEN, f));

                // send command with file size
                char textToWrite[16];
                i = header.overall_size/header.byterate;
                sprintf(textToWrite,"%lu", i);
                int len = strlen(textToWrite);
                UART_send(textToWrite, len);
                UART_send("\n", 1);
                // reset file position variables
                i = 0;
                rdnum = 0;

                //ensure that it plays and stops when it is meant to
                //BSP_AUDIO_OUT_Stop(CODEC_PDWN_SW);
                BSP_AUDIO_OUT_SetMute(AUDIO_MUTE_OFF);
                // Start the audio player, size is number of bytes so mult. by 2
                BSP_AUDIO_OUT_Play((uint16_t *)Audio_Buffer1, BUF_LEN*2);

            //handle resume files
            case(8):
                if(differentCases == 8) {
                    BSP_AUDIO_OUT_SetMute(AUDIO_MUTE_OFF);
                    BSP_AUDIO_OUT_ChangeBuffer((uint16_t*)Audio_Buffer1, BUF_LEN);
                }
                
                // loop to read
                while(r == BUF_LEN) {
                    // send command with current position
                    rdnum += 1;
                    i = (rdnum*r*header.channels)/header.byterate;
                    sprintf(textToWrite,"%lu", i);
                    len = strlen(textToWrite);
                    UART_send(textToWrite, len);
                    UART_send("\n", 1);

                    if(buff_Num == Buffer2){
                        // generate data for the audio buffer 2
                        r =	(fread (Audio_Buffer1, sizeof(int16_t), BUF_LEN, f));
                        buff_Num = Buffer1;
                    }
                    else if(buff_Num == Buffer1){
                        // generate data for the audio buffer
                        r =	(fread (Audio_Buffer2, sizeof(int16_t), BUF_LEN, f));
                        buff_Num = Buffer2;
                    }

                    //receive message
                    osMessagePut(message_Queue, buff_Num, osWaitForever);
                    osSemaphoreWait(SEMO_id, osWaitForever); //wait on semaphore

                    //process message
                    doSomething = osMessageGet (Command_FSQueue, 0);

                    //pause song
                    if(doSomething.status == osEventMessage) {
                        //if you want to pause instead of stop
                        if(doSomething.value.v == 3) {
                            BSP_AUDIO_OUT_SetMute(AUDIO_MUTE_ON);
                            break;
                        }

                        //if you want to stop song altogether
                        if(doSomething.value.v == 1) {
                            r = 0;
                        }
                    }
                    if(r < BUF_LEN) {
                        osMessagePut(mid_CMDQueue, StopFile, osWaitForever);
                        UART_send("b\n", 2); // send command to stop updating position
                        BSP_AUDIO_OUT_SetMute(AUDIO_MUTE_ON);
                        fclose (f);
                    }
                }
                break;
        }
    }
}

// Thread function
void Control(void const *arg){
    osEvent evt; // Receive message object
    Process_Event(0); // Initialize the State Machine
    while(1){
        evt = osMessageGet (mid_CMDQueue, osWaitForever); // receive command
        if (evt.status == osEventMessage) { // check for valid message
            Process_Event(evt.value.v); // Process event
        }
   }
}

void Rx_Command (void const *argument){
    char rx_char[2]={0,0};

    while(1){
        UART_receive(rx_char, 1); // Wait for command from PC GUI
        // Check for the type of character received
        if(!strcmp(rx_char,StopFile_char)){
            // Trigger1 received
            osMessagePut (mid_CMDQueue, StopFile, osWaitForever);
        } else if (!strcmp(rx_char,PlayFile_char)){
            // Trigger2 received
            UART_receivestring(receive_file, 150);
            osMessagePut (mid_CMDQueue, PlayFile, osWaitForever);
        } else if (!strcmp(rx_char,PauseFile_char)){
            // Trigger2 received
            sMessagePut (mid_CMDQueue, PauseFile, osWaitForever);
        } // end if
        if(!strcmp(rx_char,ShowFiles_char)){
            // Trigger1 received
            osMessagePut (mid_CMDQueue, ListFiles, osWaitForever);
        }
   }
} // end Rx_Command


/* User Callbacks: user has to implement these functions if they are needed. */
/* This function is called when the requested data has been completely transferred. */
void    BSP_AUDIO_OUT_TransferComplete_CallBack(void){
	osEvent evt; // Receive message object
	evt = osMessageGet (message_Queue, 0); // receive command
	if (evt.status == osEventMessage) { // check for valid message
	    osSemaphoreRelease(SEMO_id); // release semaphore
        if(evt.value.v == Buffer2){
            BSP_AUDIO_OUT_ChangeBuffer((uint16_t*)Audio_Buffer2, BUF_LEN);
        }
        else if (evt.value.v == Buffer1){
            BSP_AUDIO_OUT_ChangeBuffer((uint16_t*)Audio_Buffer1, BUF_LEN);
        }
	}
}

/* This function is called when half of the requested buffer has been transferred. */
void    BSP_AUDIO_OUT_HalfTransfer_CallBack(void){
}

/* This function is called when an Interrupt due to transfer error or peripheral
   error occurs. */
void    BSP_AUDIO_OUT_Error_CallBack(void){
	while(1){
	}
}

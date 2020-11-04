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


// Receive characters from the VB GUI
#define StopFile_char "1"
#define PlayFile_char "2"
#define PauseFile_char "3"

// Receive characters from the VB GUI
#define ShowFiles_char "4"
#define StartFileList_char "5"
#define EndFileList_char "6"
#define SendFileName_char "7"

//////////////////////////////////////////////////////////
void Thread_1 (void const *argument);             // thread function 1
osThreadId tid_Thread_1;                      // thread id 1
osThreadDef (Thread_1, osPriorityNormal, 1, 0);      // thread object 1

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
  static uint16_t   Current_State = NoState; // Current state of the SM
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
        // State2 entry actions
        LED_On(LED_Green);
      }
      if(event == ListFiles){
    	  Current_State = List;
    	  //Exit Actions
    	  LED_Off(LED_Red);
    	  //Transition actions
    	  osMessagePut(Command_FSQueue, 1, osWaitForever);
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
    		LED_On(LED_Green);
    	}
    	break;
    case Play: // PLAY
      if(event == PauseFile){
        Current_State = Pause;
        // Exit actions
        LED_Off(LED_Green);
        // Transition actions
        // State1 entry actions
        LED_On(LED_Orange);
      }
      if(event == StopFile){
        Current_State = Stop;
        // Exit actions
        LED_Off(LED_Green);
        // Transition actions
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

//Thread 1: send songs to disco board and GUI
void Thread_1 (void const *argument) {
	fsFileInfo info;
	char *StartFileList_msg = "2\n";
	char *EndFileList_msg = "3\n";

	usbStatus ustatus; // USB driver status variable
	uint8_t drivenum = 0; // Using U0: drive number
	char *drive_name = "U0:"; // USB drive name
	fsStatus fstatus; // file system status variable

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


		while(1){

			osMessageGet (Command_FSQueue, osWaitForever);
			UART_send(StartFileList_msg,2); // Send start string

			info.fileID = 0;
			int Q = 0;

			while (ffind ("U0:*.*", &info) == fsOK) {
				Q = strlen (info.name);
				UART_send(info.name, Q);
				UART_send("\n", 1);
			}
			}
		UART_send(EndFileList_msg,2); // Send start string
		osMessagePut (mid_CMDQueue, SendComplete, osWaitForever);

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
   char receive_file[150];

   while(1){
      UART_receive(rx_char, 1); // Wait for command from PC GUI
    // Check for the type of character received
      if(!strcmp(rx_char,StopFile_char)){
         // Trigger1 received
         osMessagePut (mid_CMDQueue, StopFile, osWaitForever);
      } else if (!strcmp(rx_char,PlayFile_char)){
        // Trigger2 received
         osMessagePut (mid_CMDQueue, PlayFile, osWaitForever);
      } else if (!strcmp(rx_char,PauseFile_char)){
        // Trigger2 received
         osMessagePut (mid_CMDQueue, PauseFile, osWaitForever);
      } // end if
      if(!strcmp(rx_char,ShowFiles_char)){
         // Trigger1 received
         osMessagePut (mid_CMDQueue, ListFiles, osWaitForever);
      } else if (!strcmp(rx_char,SendFileName_char)){
        // Trigger2 received
         UART_receivestring(receive_file, 150);
         rx_char[0] = 0;
      }
   }
} // end Rx_Command


// USBEjectLibrary.h

#pragma once
#include "Stdafx.h"
#include <windows.h>
#include <winioctl.h>
#include <tchar.h>
#include <stdio.h>
#include <iostream>
using namespace std;
using namespace System;

namespace USBEjectLibrary {
		public ref class EjectClass
		{
		private:
			static void ReportError(LPTSTR szMsg);

			static HANDLE OpenVolume(TCHAR cDriveLetter);

			static BOOL CloseVolume(HANDLE hVolume);

			static BOOL LockVolume(HANDLE hVolume);

			static BOOL DismountVolume(HANDLE hVolume);

			static BOOL PreventRemovalOfVolume(HANDLE hVolume, BOOL fPreventRemoval);

			static BOOL AutoEjectVolume(HANDLE hVolume);

			static string EjectVolume(TCHAR cDriveLetter);
		public:
			static String^ EjectUSBDrive(TCHAR driveLetter);
		};
	
	
}

// USBEjectLibrary.h

#pragma once
#include "Stdafx.h"
#include <windows.h>
#include <winioctl.h>
#include <tchar.h>
#include <stdio.h>
using namespace System;

namespace USBEjectLibrary {
		public ref class EjectClass
		{
		public:
			static void ReportError(LPTSTR szMsg);

			static HANDLE OpenVolume(TCHAR cDriveLetter);

			static BOOL CloseVolume(HANDLE hVolume);

			static BOOL LockVolume(HANDLE hVolume);

			static BOOL DismountVolume(HANDLE hVolume);

			static BOOL PreventRemovalOfVolume(HANDLE hVolume, BOOL fPreventRemoval);

			static BOOL AutoEjectVolume(HANDLE hVolume);

			static BOOL EjectVolume(TCHAR cDriveLetter);

			static void EjectUSBDrive(TCHAR driveLetter);
		};
	
	
}

// ֳכאגםûי DLL-פאיכ.

#include "stdafx.h"

#include "USBEjectLibrary.h"

namespace USBEjectLibrary {
		LPTSTR szVolumeFormat = TEXT("\\\\.\\%c:");
		LPTSTR szRootFormat = TEXT("%c:\\");
		LPTSTR szErrorFormat = TEXT("Error %d: %s\n");

		void EjectClass::ReportError(LPTSTR szMsg)
		{
			_tprintf(szErrorFormat, GetLastError(), szMsg);
		}

		HANDLE EjectClass::OpenVolume(TCHAR cDriveLetter)
		{
			HANDLE hVolume;
			UINT uDriveType;
			TCHAR szVolumeName[8];
			TCHAR szRootName[5];
			DWORD dwAccessFlags;

			
			swprintf(szRootName, szRootFormat, cDriveLetter); 
			

			uDriveType = GetDriveType(szRootName);
			switch (uDriveType) {
			case DRIVE_REMOVABLE:
				dwAccessFlags = GENERIC_READ | GENERIC_WRITE;
				break;
			case DRIVE_CDROM:
				dwAccessFlags = GENERIC_READ;
				break;
			default:
				_tprintf(L"Cannot eject.  Drive type is incorrect.");
				return INVALID_HANDLE_VALUE;
			}

			
			swprintf(szVolumeName, szVolumeFormat, cDriveLetter);
			
			

			hVolume = CreateFile(szVolumeName,
				dwAccessFlags,
				FILE_SHARE_READ | FILE_SHARE_WRITE,
				NULL,
				OPEN_EXISTING,
				0,
				NULL);
			if (hVolume == INVALID_HANDLE_VALUE)
				ReportError(TEXT("CreateFile"));

			return hVolume;
		}

		BOOL EjectClass::CloseVolume(HANDLE hVolume)
		{
			return CloseHandle(hVolume);
		}

		#define LOCK_TIMEOUT        10000       // 10 Seconds
		#define LOCK_RETRIES        20

		BOOL EjectClass::LockVolume(HANDLE hVolume)
		{
			DWORD dwBytesReturned;
			DWORD dwSleepAmount;
			int nTryCount;

			dwSleepAmount = LOCK_TIMEOUT / LOCK_RETRIES;

			// Do this in a loop until a timeout period has expired
			for (nTryCount = 0; nTryCount < LOCK_RETRIES; nTryCount++) {
				if (DeviceIoControl(hVolume,
					FSCTL_LOCK_VOLUME,
					NULL, 0,
					NULL, 0,
					&dwBytesReturned,
					NULL))
					return TRUE;

				Sleep(dwSleepAmount);
			}

			return FALSE;
		}

		BOOL EjectClass::DismountVolume(HANDLE hVolume)
		{
			DWORD dwBytesReturned;

			return DeviceIoControl(hVolume,
				FSCTL_DISMOUNT_VOLUME,
				NULL, 0,
				NULL, 0,
				&dwBytesReturned,
				NULL);
		}

		BOOL EjectClass::PreventRemovalOfVolume(HANDLE hVolume, BOOL fPreventRemoval)
		{
			DWORD dwBytesReturned;
			PREVENT_MEDIA_REMOVAL PMRBuffer;

			PMRBuffer.PreventMediaRemoval = fPreventRemoval;

			return DeviceIoControl(hVolume,
				IOCTL_STORAGE_MEDIA_REMOVAL,
				&PMRBuffer, sizeof(PREVENT_MEDIA_REMOVAL),
				NULL, 0,
				&dwBytesReturned,
				NULL);
		}

		BOOL EjectClass::AutoEjectVolume(HANDLE hVolume)
		{
			DWORD dwBytesReturned;

			return DeviceIoControl(hVolume,
				IOCTL_STORAGE_EJECT_MEDIA,
				NULL, 0,
				NULL, 0,
				&dwBytesReturned,
				NULL);
		}

		string EjectClass::EjectVolume(TCHAR cDriveLetter)
		{
			HANDLE hVolume;

			BOOL fRemoveSafely = FALSE;
			BOOL fAutoEject = FALSE;

			// Open the volume.
			hVolume = OpenVolume(cDriveLetter);
			if (hVolume == INVALID_HANDLE_VALUE)
				return "Can't open volume";

			// Lock and dismount the volume.
			if (LockVolume(hVolume) && DismountVolume(hVolume)) {
				fRemoveSafely = TRUE;

				// Set prevent removal to false and eject the volume.
				if (PreventRemovalOfVolume(hVolume, FALSE) &&
					AutoEjectVolume(hVolume))
					fAutoEject = TRUE;
			}

			// Close the volume so other processes can use the drive.
			if (!CloseVolume(hVolume))
				return "Can't close volume";

			if (fAutoEject) {
				string res = "OK. Media in Drive ";
				res += cDriveLetter;
				return  res+":\\ has been ejected safely.";
			}
			else {
				if (fRemoveSafely) {
					string res = "OK. Media in Drive ";
					res += cDriveLetter;
					return  res + ":\\ can be safely removed.";
				}
			}

			return "OK";
		}

		String^ EjectClass::EjectUSBDrive(TCHAR driveLetter) {
			string res = EjectVolume(driveLetter);
			string ret = res;
			if (res.find("OK")==string::npos) {
				ret = "Failure ejecting drive ";
				ret += driveLetter;
				ret += '\n' + res;
			}
			return gcnew String(ret.c_str());
		}
	
}
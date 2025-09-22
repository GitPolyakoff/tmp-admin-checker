# TMP Admin Checker
TMP Admin Checker is a small program for **TruckersMP** (ETS2 / ATS). It helps you detect admins near you in the game using game logs.

---

## What it does
- Reads your TruckersMP game logs.
- Checks if any admins are online near you.
- Shows notifications on your desktop when admins appear.
- Creates a file on your desktop with a list of all admins from the official TruckersMP team page.

---

## How to use
1. Download or clone the repository.
2. Run the program file: `tmp-admin-checker.exe`.
3. On the first run, the program will ask you to **choose your game log file**.
- Example: `C:\Users\User\Documents\ETS2MP\logs\log_spawning_xxxx.xx.xx_log.txt`.
4. The program will **create a file on your desktop** with all admins.
5. The checker will start monitoring your logs and show notifications when an admin is nearby.

---

## Screenshot of a work example
<img width="1255" height="772" alt="2" src="https://github.com/user-attachments/assets/fc6c5670-341f-42df-95bf-989534235f15" />

---

## Notes
- The **program updates the list of admins automatically** from TruckersMP website.
- Notifications include the **admin’s display name, in-game ID, roles, and tag**.
- You can **change the log path** by editing the saved file on your desktop `TMPCheckerPaths.txt`.

---

## Requirements
- Windows 10 OS or higher
- .NET Framework 4.7.2 or higher
- Internet connection (to update the admin list)

---

## Developer info

- You can check the source code on GitHub: [tmp-admin-checker](https://github.com/GitPolyakoff/tmp-admin-checker)
- Feel free to modify or improve the program.

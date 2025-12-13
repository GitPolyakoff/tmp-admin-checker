# TMP Admin Checker
TMP Admin Checker is a small program for **TruckersMP** (ETS2 / ATS). It helps you detect admins near you in the game using game logs.

---

## What it does
- Automatically detects your TruckersMP game log file.
- Reads TruckersMP game logs in real time.
- Checks if any admins are online near you.
- Shows desktop notifications when an admin appears nearby.
- Creates a file on your desktop with a list of all admins from the official TruckersMP team page.

---

## How to use
1. Download or clone the repository.
2. Run the program file: `tmp-admin-checker.exe`.
3. On first launch, the program will **automatically find your game log file**.
4. The program will **create a file on your desktop** containing the list of all admins.
5. The checker will start monitoring your logs and show notifications when an admin is nearby.

---

## Screenshot of a work example
<img width="1129" height="748" alt="image" src="https://github.com/user-attachments/assets/339df06f-9563-4f33-8f21-a76a76ce0a41" />

---

## Notes
- The **admin list is updated automatically** from the TruckersMP website.
- Notifications include the **admin’s display name, in-game ID, roles, and tag**.
- If automatic log detection fails, you can **manually set the log path** by editing the file on your desktop `TMPCheckerPaths.txt`.

---

## Requirements
- Windows 10 OS or higher
- .NET Framework 4.7.2 or higher
- Internet connection (to update the admin list)

---

## Developer info

- You can check the source code on GitHub: [tmp-admin-checker](https://github.com/GitPolyakoff/tmp-admin-checker)
- Feel free to modify or improve the program.

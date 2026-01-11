# TMP Admin Checker

TMP Admin Checker is a small tool for **TruckersMP (ETS2 / ATS)**.  
It helps you detect **admins near you in-game** by reading TruckersMP log files in real time.

<div align="center">
  <img src="https://visitor-badge.laobi.icu/badge?page_id=GitPolyakoff.tmp-admin-checker" />
  <img src="https://img.shields.io/github/stars/GitPolyakoff/tmp-admin-checker?style=flat&logo=github&label=stars" />   
</div>

---

## What it does

- Automatically finds your TruckersMP **spawning log**.
- Reads game logs in real time while you play.
- Detects when **one or more admins** appear near you.
- Shows **desktop popup notifications** when an admin is detected.
- Plays a **continuous warning sound** while at least one admin is nearby.
- Saves all detected admin encounters to a log file.
- Automatically downloads and updates the **official TruckersMP admin list**.
- Provides a **settings panel** to control sounds and notifications.

---

## Features

- Popup notifications (can be enabled or disabled).
- Popup notification sound (can be enabled or disabled).
- Admin nearby sound (can be enabled or disabled).
- Shows the **last detected admin** in the main window.
- Sound keeps playing as long as **at least one admin is near you**.
- Admin roles are verified using the **official TruckersMP team page**.

> ⚠ The admin nearby sound detection is in **beta**.  
> In some situations, the game may not log updates while the admin is standing still, which can affect detection timing.

---

## Files & storage

All temporary files are stored in one folder: `Downloads\tmp-admin-checker`

This folder contains:
- `admins.txt` – list of all TruckersMP admins
- `AdminMeetingsLog.txt` – history of detected admins

These files are **automatically created, updated, and overwritten** when the program starts.  
You no longer need to delete anything manually.

---

## How to use

1. Download the latest release from the **Releases** page.
2. Start **TruckersMP**
3. Run `tmp-admin-checker.exe`.
4. Click **Start checker** in TMP Admin Checker.
5. When an admin appears near you, you will see a popup and hear a sound.

---

## Screenshots

<img width="341" height="453" alt="image" src="https://github.com/user-attachments/assets/89af1572-15a1-47de-a608-f5f3926c8662" />

<img width="601" height="278" alt="screen" src="https://github.com/user-attachments/assets/05f7c9fd-cf1a-48f1-b927-71dfda0866b2" />

---

## Notes

- It is recommended to start **TruckersMP first**, and only then launch **TMP Admin Checker**.
- Admin data is loaded from the [official TruckersMP website](https://truckersmp.com/team).
- All detection is based on **TruckersMP spawning logs**.

---

## Requirements
- **Windows 10 OS** or higher
- **.NET Framework 4.7.2** [Download](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net472)

# 🏥 Medical Appointment Management System

A full-stack medical appointment management web application built with **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**, designed to simulate a real-world healthcare platform with secure, role-based access for **Users**, **doctors**, and **administrators**.

---

## 📌 Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Doctor Role](#doctor-role)
- [User Role](#patient-role)
- [Admin Role](#admin-role)
- [Tech Stack](#tech-stack)
- [Screenshots](#screenshots)
- [Future Improvements](#future-improvements)


---

## 🧠 Overview

This project enables patients to search for doctors, browse clinics, and book medical appointments online. Doctors can manage their clinics, availability, and appointments, while admins handle patient complaints and ensure proper resolution workflows.

The system uses **Leaflet.js** for dynamic clinic location selection and **MailKit** for email notifications.

---

## 🚀 Features

### 🩺 Doctor Role
- Create & manage a detailed **professional profile**:
  - License number & image proof
  - Specialization, experience, education
  - Affiliation, department, position
  - Bio and emergency contact info

- Manage multiple **clinics**:
  - Add name, phone, working hours, and working days
  - **Select exact clinic location using Leaflet maps**
  - Edit or remove clinic information
  - Upload clinic images
  - Patients can view the location directly in **Google Maps**

- Schedule **availability time slots** per clinic:
  - Add/edit/remove available appointment times
  - System validates against clinic working hours
  - Prevents overlapping time slots

- Handle **appointment requests**:
  - View patient info and uploaded documents (PDF only)
  - Accept or reject requests with optional reasons
  - Automatic email notifications to patients

- Respond to **patient complaints**:
  - View complaint details & attachments
  - Submit response with tracked status updates

---

### 👨‍⚕️ User Role
- Browse/search **clinics and doctors**:
  - Filter by name or specialization
  - View full clinic & doctor details

- Book appointments:
  - Choose from doctor’s available slots
  - Provide chronic disease info, medical complaints
  - Upload supporting PDFs (e.g., prescriptions, x-rays)
  - View status (Pending, Approved, Withdrawn)

- Manage requests:
  - Cancel bookings before approval
  - Automatic deletion of documents on withdrawal

- File & track **complaints**:
  - Submit complaint details and attachments
  - Monitor status and doctor responses
  - Withdraw complaints if needed

---

### 🛡️ Admin Role
- Access global complaint dashboard
- View all patient complaints system-wide
- Forward complaints to doctors via email
- Track doctor responses and resolution status

---

## 🔧 Tech Stack

| Layer            | Technology                          |
|------------------|--------------------------------------|
| Language         | C#                                   |
| Framework        | ASP.NET Core MVC                     |
| Database         | SQL Server                           |
| ORM              | Entity Framework Core                |
| Authentication   | ASP.NET Identity                     |
| Email            | MailKit                              |
| Maps             | Leaflet.js                           |
| Uploads          | PDF-only with secure validation      |
| Architecture     | Layered (MVC, ViewModels, RBAC)      |

---

## 📷 Screenshots

![alt text](<Screenshot (243).png>) ![alt text](<Screenshot (234).png>) ![alt text](<Screenshot (235).png>) ![alt text](<Screenshot (236).png>) ![alt text](<Screenshot (237).png>) ![alt text](<Screenshot (238).png>) ![alt text](<Screenshot (239).png>) ![alt text](<Screenshot (240).png>) ![alt text](<Screenshot (241).png>) ![alt text](<Screenshot (242).png>)

---

## 🔮 Future Improvements

- 💳 Integrate **payment gateway (e.g. Stripe)** to charge appointment fees
- 📊 Add doctor & admin **dashboard with analytics/charts**
- 🔔 Implement **email or SMS reminders** for upcoming appointments
- 📅 Add **calendar view** for appointment scheduling
- 🗂 Enable **PDF export** for reports and appointment summaries

---

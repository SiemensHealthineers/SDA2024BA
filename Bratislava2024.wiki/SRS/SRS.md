# Software Requirements Specification - NeuroMedia (SHS SDA2024 project)

## 1. Introduction

           1.1 Purpose
                Purpose of this document is to document basic information about Software Requirements Specification.

           1.2 Intended Audience
                Project stakeholders, product owner, scrum master, developers, testers.

           1.3 Intended Use
                Get basic information about requirements of the intended software product.

           1.4 Product Scope
                Dystonia is one of the frequent diagnoses in neurology. Patients with dystonia
                have involuntary muscle spasms and contractions. These lead to twisting or jerky movements
                and unusual body positions.

                Cervical dystonia is manifested by contractions of the neck muscles that lead to abnormal twisting,
                bowing, or turning the head, which are often combined with tremors or jerky movements.
                Treatment in this case consists of intramuscular administration of botulinum toxin and long-term treatment
                monitoring the patient's condition. The effect of botulinum toxin is best assessed with a one-month interval after application.           

                However, due to the busyness of the neurological clinics, the patient often comes for a check-up too late (waiting times 
                are realistically 3 or more months). As a result, the effect of botulinum toxin application cannot be traced either
                set up the correct ongoing treatment.

                The proposed application (product) solves the remote control of the patient's condition by recording of
                videos, tracking and categorizing patient status and automation of some activities (control reminders, 
                tracking over time, tracking medications, etc.).

                In general, the product will consist of a web application (will be used mostly by doctors) and by mobile
                app (used by patients to deliver videos and questionnaires/scales).
                The whole product will be used for remote monitoring of the patient and for effective setup of medical
                startegy proposed by doctor.

           1.5 Definitions and Acronyms
                Webapp - web application
                TWSTRS - Toronto Western Spasmodic Torticollis Rating Scale
                CGI - scale for gathering patient state after application of botulinum toxin.

## 2. Overall Description

           2.1 User Needs
                Due to the busyness of the neurological clinics, the users (both doctors and patients) need a tool to improve the 
                effectiveness and time saving of the patient monitoring. The solution is the remote patient monitoring available 
                by this product.

           2.2 Assumptions and Dependencies
                We assume that the patient is willing to be monitored by mobile app and that the whole process can save time and 
                effort for both sides (doctor and patient).
                The patient needs to have access to the mobile phone and install the mobile app. The doctor 
                needs to have internet access.
                The web app will be hosted on Azure, in case of disaster/unavailability, it will fail.
                Both webapp and mobile app rely on using Azure AD, in case of unavailability, logging in of the user login.
                The mobile app depends on the availability of the webapp to fetch the data, in case of unavailability its functionality 
                will fail

## 3. System Features and Requirements

            3.1 Functional Requirements
                - User login/logout (medical staff, patients). User needs to be authenticated by Azure AD.
                - See patients [list, CRUD]
                - See patient Visits [list, CRUD]
                - See patient questionnaires/scales (TWTRS, CGI, botulotixine application) [list, CRUD]
                - See videos of patient [list, CRUD]]
                - Send email and push notifications
                - Record short videos of patient (by mobile app) [C]

            3.2 External Interface Requirements
                - internet connection for both webapp and mobile app
                - patient has to have access to a mobile phone (planned: Android, nice to have: iOs)
                - access to Azure AD for user management
                - mobile app needs to have access to webapp API

            3.3 System Features
                - system language: English
                - roles: Patient, Nurse, Doctor, Admin, SystemManager
                - patient data anonymization
                - patient Information: name, surname, email, birth year, gender, diagnosis, notes
                - questionnaires: TWSTRS-2, CGI, botulinum toxin application (calculate scores)
                - follow-up: patient returns after 3 months, but a video of the patient is reviewed after 1 month. Additionally, another video is recorded at the doctor's office. Appointment is scheduled for 3 months later.

            3.4 Nonfunctional Requirements
                - Security and Privacy:
                    The system maintain fine level of security against unauthorized access, authorization
                    and authentication is provided by Azure AD, because each user has an account that holds
                    personal information.
                    In addition, the system website includes an SSL certificate which enables the website to
                    run securely on https protocol.
                - Efficiency and Usability
                    This system ensures a high level of efficiency which will be accomplished through multiple
                    levels of testing and iteration of the user experience flow and by periodic stakeholders feedback.
                - Scalability and Performance
                    The system is designed with performance and scalability in mind, Azure platform is
                    built for performance and scalability with it’s general infrastructure possibilities.

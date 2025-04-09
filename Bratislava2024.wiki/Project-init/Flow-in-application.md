**General Flow: Application at the Doctor's Office**
- Information on Diagnosis
- Muscles Targeted by the Injection
- Patient Information: 
     - Name, surname, email, birth year, gender, diagnosis, notes
- Substance Administered
- TWSTRS-2: Complete questionnaire, calculate score (need for decision support machine?)
- Determine what data can be used
- Visit Record: Administration of botulinum toxin
- Follow-Up: Patient returns after 3 months, but a video of the patient is reviewed after 1 month
     - Additionally, another video is recorded at the doctor's office 
     - Appointment scheduled for 3 months later


___________________________________________________________________________________________________
**Additional Information**
- Entire system in English
- Need a table for botulinum toxin application: Calculate total dose
- TWSTRS-2 Scale
- CGI Scale: Also part of the mobile application
- Roles:
	- Assistant (Nurse)
	- Doctor
	- Admin
	- Patient
- Video from front and side, does not need to be at the same time, 15s + 15s
- Pair the mobile application with the patient for the first time at the clinic and then automatically afterward
- Preferably send push notifications

**- After One Month:**
- Patient completes the CGI scale via the mobile app
- Allow video recording (limit sending, e.g., every half hour)
- First, record video, then CGI
- Only admin can delete (everything)
- Email notifications only to the patient; doctor does not need anything
- Mechanism to send push notifications and emails 3 days before and after the due date until the video is recorded
___________________________________________________________________________________________________

**Data Storage:**
- Patient Information
- Visits
- Questionnaires with medical information (e.g., botulinum toxin application information (dose received), CGI, TWSTRS-2)
- Video
- Anonymized Data: Must not have access to real data (separate databases? Real patient ID linked to randomized ID)
- Doctors can access real data, but access needs to be controlled

# SAHWebsite - Humber 5204 Group 5 - Group Members: Will Midgette, Ikumi Mine, Danyal Effendi, Bakary Diarra, Barbara Cam, Mahsa Fard

## This is a school project. Our group of 6 members has proposed new changes to the SAH hospital website in Sault Ste Marie, Ontario. These changes are structured at the MVP level in this submission, in which each group member is creating CRUD functionality for 2 database entities. 
 
### Will - Chats and Messages database entities 
- For the MVP I have created CRUD functionality for the chat and messages entities. A user can create, read and/or delete a chat. A user can read, update, create and/or delete a message. The details view for a chat shows all of the messages associated with a chat. This essentially doubles as the list view for messages. 

### Ikumi - Departments, Specialities, and Donations database entities
- MVP : a user can create, read, update and delete each entities respectively. The donation table contains data from the departments and users table, so a user also can see username and department name in the donation update, details, and delete confirm views. Displaying usernames and department names in the donation list view is still in progress.
- Updates : 
  - The donation table’s user class is extended into the ApplicationUser class.
  - Donor and department names are now displayed on the donation list page. 
  - A list of donations(donor name) is displayed on the department details page.
  - A logged-in user as only admin can operate all CRUD operations for all tables.
  - A logged-in user as a patient (we set it as a default role) or donor can make a new donation.
  - Validation is added but only for the fields that are populated by Razor.
  - All pages are responsive for desktop, tablet, mobile screen size.
- Things I tried but not completed yet :
  - Displaying donation information on the user account page. 
    - The issue and what I have done : I got an error about the data (JSON object) I tried to get from the database that cannot be deserialized into IEnumerable<DonationDto> that requires a JSON array. As the error message said, I tried to convert data to JSON array by using jss.DeserializeObject or JSON.Parse. However, due to my lack of knowledge of JSON, the usage and syntax didn’t work. I need to learn about JSON more.
  - Updating donation data operation by admin is not working.
    - The issue and what I have done : The parameter “id” of the edit method in DonationController is non-nullable, but the parameters dictionary contains a null entry. I made sure the update method in DonationDataController worked through curl request, the name attribute for all form elements are used, the post request is not sent properly. I wrote an explicit data model to be stored in the database in DonationController but it was the same result.
- Extra :
  - I recieved supports from all of my teammates.
  
### Bakary - Parking spots and tickets database entities
- The CRUD (create,read, update and delete) functionalities are functioning for the two entities. The user can also see on a details page the relationships between the two entities and the user table. Currently any user can realise the CRUD actions but role based authentication and authorization will be implemented in the next step to restrict the user actions depending on his/her role.
- In the current version, only Admin can do CRUD for parking spots entity
- JS validation for all the entities

### Barbara - Jobs and Applications database entities
- My MVP has create, read, update and delete fuctionality for the application and job entities. The application table is a bridge table for users table and jobs table. The application(details) will provide all the information from the job and the user as well. Views for the application table are still in process as part of the application's controller havent been verified.

### Danyal - Internal Education Portal - Courses and EmployeeApplications
- This feature is for staff who can enroll in any of the listed courses. The CRUD for courses is completed, which will be for admin only to add, edit or remove the courses. The user authentication will be implemented lated after completion of CRUD for all features.


### Mahsa - Frequently Asked Question (FAQ) - Book Appointment
-  FAQ : It has CRUD for the admin side( create, Read, Update and delete) and for the user side Create and Read. Users can submit the question. Admin receives the query and answers them. Besides, the admin can add new questions with answers and even update the existing asked questions.
-   Book appointment : In this feature user can request for booking an appointment(Create) and Admin user can see the list of the appointments requests, change the date and time, or cancel appointments. Appointment table contains data from the department and user table. The user table contain doctors information when the role is doctor.

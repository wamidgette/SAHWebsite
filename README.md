# SAHWebsite - Humber 5204 Group 5 - Group Members: Will Midgette, Ikumi Mine, Danyal Effendi, Bakary Diarra, Barbara Cam, Mahsa Fard

## This is a school project. Our group of 6 members has proposed new changes to the SAH hospital website in Sault Ste Marie, Ontario. These changes are structured at the MVP level in this submission, in which each group member is creating CRUD functionality for 2 database entities. 
 
### Will - Chats and Messages database entities 
- For the MVP I have created CRUD functionality for the chat and messages entities. A user can create, read and/or delete a chat. A user can read, update, create and/or delete a message. The details view for a chat shows all of the messages associated with a chat. This essentially doubles as the list view for messages. 

### Ikumi - Departments, Specialities, and Donations database entities
- For the MVP, a user can create, read, update and delete each entities respectively. The donation table contains data from the departments and users table, so a user also can see username and department name in the donation update, details, and delete confirm views. Displaying usernames and department names in the donation list view is still in progress.

### Bakary - Parking spots and tickets database entities
- The CRUD (create,read, update and delete) functionalities are functioning for the two entities. The user can also see on a details page the relationships between the two entities and the user table. Currently any user can realise the CRUD actions but role based authentication and authorization will be implemented in the next step to restrict the user actions depending on his/her role.

### Barbara - Jobs and Applications database entities
- My MVP has create, read, update and delete fuctionality for the application and job entities. The application table is a bridge table for users table and jobs table. The application(details) will provide all the information from the job and the user as well. Views for the application table are still in process as part of the application's controller havent been verified.

### Danyal - Internal Education Portal - Courses and EmployeeApplications
- This feature is for staff who can enroll in any of the listed courses. The CRUD for courses is completed, which will be for admin only to add, edit or remove the courses. The user authentication will be implemented lated after completion of CRUD for all features.

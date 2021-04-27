# SAHWebsite - Humber 5204 Group 5 - Group Members: Will Midgette, Ikumi Mine, Danyal Effendi, Bakary Diarra, Barbara Cam, Mahsa Fard

## This is a school project. Our group of 6 members has proposed new changes to the SAH hospital website in Sault Ste Marie, Ontario. These changes are structured at the MVP level in this submission, in which each group member is creating CRUD functionality for 2 database entities. 
 
## Will - Chats and Messages database entities 
- For the MVP I have created CRUD functionality for the chat and messages entities. A user can create, read and/or delete a chat. A user can read, update, create and/or delete a message. The details view for a chat shows all of the messages associated with a chat. This essentially doubles as the list view for messages. 

## Ikumi - Departments, Specialities, and Donations database entities
### MVP
- A user can create, read, update and delete each entities respectively. The donation table contains data from the departments and users table, so a user also can see username and department name in the donation update, details, and delete confirm views. Displaying usernames and department names in the donation list view is still in progress.
### Updates  
- The donation table’s user class is extended into the ApplicationUser class.
- Donor and department names are now displayed on the donation list page. 
- A list of donations(donor name) is displayed on the department details page.
- A logged-in user as only admin can operate all CRUD operations for all tables.
- A logged-in user as a patient (we set it as a default role) or donor can make a new donation.
- Validation is added but only for the fields that are populated by Razor.
- All pages are responsive for desktop, tablet, mobile screen size.
### Things I tried but not completed yet 
- Displaying donation information on the user account page. 
  - The issue and what I have done : I got an error about the data (JSON object) I tried to get from the database that cannot be deserialized into IEnumerable<DonationDto> that requires a JSON array. As the error message said, I tried to convert data to JSON array by using jss.DeserializeObject or JSON.Parse. However, due to my lack of knowledge of JSON, the usage and syntax didn’t work. I need to learn about JSON more.
- Updating donation data operation by admin is not working.
  - The issue and what I have done : The parameter “id” of the edit method in DonationController is non-nullable, but the parameters dictionary contains a null entry. I made sure the update method in DonationDataController worked through curl request, the name attribute for all form elements are used, the post request is not sent properly. I wrote an explicit data model to be stored in the database in DonationController but it was the same result.
### Other  
- I recieved supports from all of my teammates.
  
## Bakary - Parking spots and tickets database entities
- The CRUD (create,read, update and delete) functionalities are functioning for the two entities. The user can also see on a details page the relationships between the two entities and the user table. Currently any user can realise the CRUD actions but role based authentication and authorization will be implemented in the next step to restrict the user actions depending on his/her role.
- Updates
  - Parking spot entity
    - Only Admin can do Create, edit and delete for parking spots entity
    - other users can only read parking spots
  - Tickets entity
    - Role based rendering on all actions
    - Only Admin can CRUD all the tickets 
    - Other logged in users can CRUD only their own tickets
  - JS validation for all the entities
  - Learnt a lot from this very dynamic team
  - Deepened my understanding about the use of GitHub

## Barbara - Jobs and Applications database entities
- My MVP has create, read, update and delete fuctionality for the application and job entities. The application table is a bridge table for users table and jobs table. The application(details) will provide all the information from the job and the user as well. Views for the application table are still in process as part of the application's controller havent been verified.

### Jobs
 This portal will display all job position availables and details. The CRUD is working. The job list and the details can be accesed by everyone. The create, delete and 
 update functionality only applies to Admin role.
    
### Application
 The user can apply for jobs from the Job List. Three roles exist Patients, Applicants and Admin. The Edit, details and delete functionality is only available for the Admin. The Create only for Applicants. I had so many issues making work with the roles. I commented in order to allow the project to run.
 
 ### Updates
 
 - Improve qualitative code, e.g creating getusers from the data controllers, better indentation and cleaner
 - Include reference and Improve comments
 - Swicth from the Ourusers table to ASP.Net users table
 - Add Roles
 - This time all crud is working except update for Applications(after switching table)
 - Add some Admin privileges
 
### Teamwork
 I would like to use this opportunity to mention about all the suport and knowledge from my Team Members through chats, discord meeting and more. However, 
    Two team member went above and beyond: Diarra and Danyal. They were extremely support and helpful, answering so many question ( a lot), helping me to debug, 
    giving me clues as they were extremely patients with me. Saturday I was feeling a frustrated and Diarra told me to do not give up!. Also Danyal was giving me 
    many ideas where to check my errors .Net is still my weakness but having great team meambers allow me to grow and feel more comfortable. 

## Danyal - Internal Education Portal - Courses and EmployeeApplications
- This Internal Education Portal feature is for staff who can register/enroll in any of the listed courses. It has 2 CRUD functionalities i.e. Courses & EmployeeApplications. - The role authorization is implemented as follows:
### For Courses
 - Anyone can see the course list along with only two possible action, i.e. Register & Details.
 - Admin can add new course, update or delete any course.
 - Only role “staff” can register for course that will direct them to Create function of EmployeeApplicants
### For Employee Applications
 - This section is mainly for Admin with only one exception. Staff can register for a course that means it can create new application.
 - Staff can only see details of application after applying
 - Admin can view, delete or update employee applications as well as register new on behalf of any staff member.
### Updates from Feedback
 - Added additional data in courses like Course Duration and Start Date.
 - In course details, user can see how many applications exists for specific course and Admin can see registered ApplicationId in the list
 - In EmployeeApplication details, admin can see basic user information as well as the course applied in the specific application
 - Added additional field of reason for applying for course
 - Created GetCourseForApplication method to get details of course and link the course in EmployeeApplication Details
### Failed Attempts/Future Implications
 - I tried to create a separate view “Register” with respective method for staff who wants to register for the specific course details page. I passed the coursed through GET and tried getting id of logged in user to create application without inputting any data. Unfortunately, by mistake I deleted that method instead of commenting out.
 - I also tried to show User names registered for course instead of EmployeeApplicantId in Course Details.
 - I created the method GetApplicationsForUser to show all the application user has applied for the courses in the User profile page. I am missing something and unable to figure out.

### Improvements & Contribution
 - I added the datepicker functionality through JQuery UI and also helped other team members to implement it.
 - First added role based rendering and helped other team members in implementing
 
### Teamwork
 - Every team member went beyond to help each other in debugging and making things work. 
 - Will and Bakary contributed more in setting up the project.



## Mahsa - Frequently Asked Question (FAQ) - Book Appointment
-  FAQ : It has CRUD for the admin side( create, Read, Update and delete) and for the user side Create and Read. Users can submit the question. Admin receives the query and answers them. Besides, the admin can add new questions with answers and even update the existing asked questions.
-   Book appointment : In this feature user can request for booking an appointment(Create) and Admin user can see the list of the appointments requests, change the date and time, or cancel appointments. Appointment table contains data from the department and user table. The user table contain doctors information when the role is doctor.

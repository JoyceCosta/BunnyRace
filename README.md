# BunnyRace 

 It is a simple but very interesting example as it shows the use of not only one but several Threads within an application.
 
 It shows how we can use some techniques for synchronization, in this case two techniques are used, the CountdownEvent() for the main Thread to wait, then this Thread calls a Wait() and waits, and each time a thread passes, it calls the Signal() method to warn that this Thread has already been terminated.
 
 And the other technique used was the use of Lock() to prevent more than one Thread from accessing the Add() method of the list at the same time, which could cause inconsistency problems when inserting elements.

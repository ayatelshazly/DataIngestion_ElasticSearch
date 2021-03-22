

How was I thinking :
=================

1.	My Idea is : First we need to read all Files 
2.	Second by reading artist_Collection Table we can connect them together
3.	then I start to feed the Album Model with the correct data (Collection , artist , UPC)  , in which every Album has its own collection and List of artists
4.	after feeding the album i start to Publish it to another Service and then Subscribe and inject Album To Elasticsearch 
5.	I made two microservices , APIs  :
-	One for reading tables and feeding album with data  ,this service responsible for Publishing Album , the name of the service  =>  DataIngestion.PublishAlbum
-	The other one for injecting Album into Elastic Search , this service responsible for subscription , the name of the service  =>   DataIngestion.SubscribeAlbum
6.	there's another Solution (Messaging) , that holds all the Events , Implementation of the EventBus , PublishEvent , this solution is shared between my Two microservies
7.	applied CQRS Pattern for two services 
8.	I used Dapr for messaging or Pub/sub between service.
9.	IntegerationTest /UnitTest Applied for both services 

What I was willing to do after this stage ?
=================================

•	to read all this files in parallel Tasks , it will make a big difference in the performance ..


How to install Dapper on your machine 
=================================================

1.	you need to install Docker .
2.	on windows machines .
3.	open PowerShell and copy, run this below command as it is :

•	powershell -Command "iwr -useb https://raw.githubusercontent.com/dapr/cli/master/install/install.ps1 | iex"

4.	From docker setting switch to Linux container  
5.	open command Prompt and Make sure that you run Command Prompt as administrator (right click, run as administrator)
6.	on the command prompt copy and past this below command
•	dapr init
7.	and that's it 
8.	if you face any problem in installation visit again this site = >  https://docs.dapr.io/getting-started/install-dapr-selfhost/ 

How to run the project on your machine 
================================
1.	Open DataIngestion.PublishAlbum , open command Prompt on the path of the solution , copy and paste this below Command ..Note (8000 is dapr port , 16334 service port)
•	dapr run --app-id pubservice --app-port 16334 --dapr-http-port 8000 dotnet run

2.	Open DataIngestion.SubscribeAlbum , open command Prompt on the path of the solution , copy and paste this below Command .. ..Note (6000 is dapr port , 61665 service port)
•	dapr run --app-id subservice --app-port 61665 --dapr-http-port 6000 dotnet run

3.	 Call this Below API , and pass valid path to it , please add extension .txt to files 
•	http://localhost:16334/api/PublishAlbum?readingPath="../../../ValidDataTestFiles/" 

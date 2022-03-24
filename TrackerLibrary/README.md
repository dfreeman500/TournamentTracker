## Tournament Tracker

This project was made using Tim Corey's ~24-hour long C# tutorial found at https://youtu.be/wfWxdh-_k_4 

Even though it is very long, I recommend this tutorial because he goes in great detail regarding his rationale for his decision-making.



Instructions:
* Clone Repo
* May need to install Dapper
* Set persistent storage (either db or csv) in Program.cs 
    * For line: TrackerLibrary.GlobalConfig.InitializeConnections(TrackerLibrary.DatabaseType.XXX);
    * Set XXX to 'TextFile' or 'Sql'
* Enjoy using the Tournament Tracker







Future Directions: Add SQL capability (right now persistent storage is only through csv)
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Roommates.Repositories
{
    ///<summary>
    ///  API MANAGER
    ///  This class is responsible for interacting with Roommate data.
    ///  It inherits from the BaseRepository class so that it can use the BaseRepository's Connection property and connectionString paramater 
    ///  // that paramater is used here and the base Repository, it is actually called in the base repository?
    /// </summary>
    class RoommateRepository : BaseRepository
    {
        /// <summary>
        /// The Constructor below is REQUIRED to handle the "connection string" and then " : " hande it off to the base class
        /// when somone makes a new room mate repository they are going to hand this class a string and then i will pass it along to base class
        /// // the connectionstring is the address of the database 
        /// // connection string is set in the top of Program.cs
        ///  ...When new RoomRespository is instantiated, pass the connection string along to the BaseRepository
        /// </summary>
        public RoommateRepository(string connectionString) : base(connectionString) { }

        // ...We'll add some methods shortly...
        /// <summary>
        ///  Get a list of all Rooms in the database
        /// </summary>
        public List<Roommate> GetAll()
        {
            //  We must "use" the database connection.
            //  In C#, a "using" block ensures we correctly disconnect from a resource even if there is an error.
            //  For database connections, this means the connection will be properly closed.
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, Firstname, Lastname, RentPortion, MoveInDate, RoomId FROM Roommate";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the rooms we retrieve from the database.
                    List<Roommate> roommates = new List<Roommate>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int FirstnameColumnPosition = reader.GetOrdinal("Firstname");
                        string FirstnameValue = reader.GetString(FirstnameColumnPosition);

                        int LastnameColumnPosition = reader.GetOrdinal("Lastname");
                        string LastnameValue = reader.GetString(LastnameColumnPosition);

                        int RentPortionColumnPosition = reader.GetOrdinal("RentPortion");
                        int RentPortionValue = reader.GetInt32(RentPortionColumnPosition);


                        int MoveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                        DateTime MoveInDateValue = reader.GetDateTime(MoveInDateColumnPosition);

                        int Room = reader.GetOrdinal("RoomId");
                        int RoomIdValue = reader.GetInt32(Room);

                        //initally being set as null inside the object so it does not need to be declared outside of it.
                      
                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            Firstname = FirstnameValue,
                            Lastname = LastnameValue,
                            RentPortion = RentPortionValue,
                            MoveInDate = MoveInDateValue,
                            Room = null,
                            

                        };

                        // ...and add that room object to our list.
                        roommates.Add(roommate);
                    }


                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of rooms who whomever called this method.
                    return roommates;
                }
            }
        }

        /// <summary>
        ///  Returns a single roommate with the given id, either null or the row represnted by this column id .
        ///  public TYPE of METHOD(id number)
        /// </summary>
        public Roommate GetById(int id)
            //requires using.Roommates.Models 
        {
                    //we cant make a connection and leave it open because we would run out of resources.. like any other natural resource (finite resource)
                    //when we close out this using statement the connection is closed,
            using (SqlConnection conn = Connection)
                //requires Microsoft.Data.SqlClient (someimtes VS adds it for you when you entaniate it)
                //Connection is inherited from the base repository class
            {
                //open the connection "tunnel", it does not REQUIRE a close command..(reader requires a close though)
                conn.Open();

                    //like a train cart "tunneling" into a gold mine to extract the gold 
                    //a physical connection, an extrernal happening, so it usees a USING BLOCK,
           
                using (SqlCommand cmd = conn.CreateCommand())
                    //using "cmd" is a conventional name for commands
                {
                    /*@"SELECT rm.Id,
                     *          rm...
                     *          r.Name
                     *          r.MaxOccupancy
                     *          r....
                     *          FROM Roommaterm LEFT JOIN Room r ON r.id= Rm.RoomId
                     *          Where rm.id =@id
                     *          ...if you ever have to deal with ambiguous column names you have to use an alias (disambiguate)... such as rm.Id and r.Id , if you ever needed to call on them you would need to make them an alias so it would be "rm.Id AS IDFROMROOM" "r.id AS IDFROMROOMMATE"
                        */
                    //do a SQL query test to see what column names you have in the table and then COPY AND PASTE IT INTO 1 line (or @)
                    // dont forget to add your WHERE statement into here, but it looks differnt than sqlquery 

                    //EDIT THIS SQL QUERY TO INCLUDE ROOM TABLE
                    cmd.CommandText = @"SELECT rm.Id,
                        rm.LastName,
                        rm.FirstName,
                        rm.RentPortion,
                        rm.MoveInDate,
                        rm.RoomId AS RoomId,
                        r.Id AS IdOfRoom,
                        r.Name,
                        r.MaxOccupancy
                        FROM Roommate rm LEFT JOIN Room r ON r.id = Rm.RoomId
                        Where rm.id = @id";

                    // to getbyId paramater Id you need to declare it will the paramater values (paramaters.addwithvalue), dontforget the @ sign
                    cmd.Parameters.AddWithValue("@id", id);
                    //
                    //need to execute it by send it down the tunnel and tell it to run this query on this database.
                    //this command is happening inside the sql server database..visual studio is acting as a client for sql server (sql server is a service running  in the background of our computer / server , it is not part of visual studios)
                    SqlDataReader reader = cmd.ExecuteReader();
                    //set that command as a variable called "reader" that will give US access to the data it captured.


                    // enstantiate a new room mate to hold the "If True" response values
                    Roommate roommate = null;

                    //if reader.Read returns an "empty response" then it did not find any results..if false
                    //if reader.Read return a response then it found a record... if true
                    // If we only expect a single row back from the database, we don't need a while loop.
                    //REUQIRES A CLOSE
                    if (reader.Read())
                    {
                       //take that roomamte object enstantiated above and set all the values of our roommate properties by using GetOrdinal method on the reader command and pass in "columnname" 
                       //GetOrdinal returns the postion based on zero index of the RESULTS SET (NOT Necessarily THE TABLE POSITON)
                       //encapsulate the getOrderinal method in a reader that uses a method to parse thast response
                        roommate = new Roommate()
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("Firstname")),
                            Lastname = reader.GetString(reader.GetOrdinal("Lastname")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            //anytime you have a variable that is defined as a class it can be defined it as null
                            //Room = null,
                            Room = new Room()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                            }
                        };
                    }

                    reader.Close();
                    //if you return say roommate.Firstname and the response is null, it will blow up and throw an exception
                    return roommate;
                }
                //if you do a "return null;" here it will satisfy the possible null "empty response" BUT the close for the reader is inside the IF statement, you could dupliate the reader.Close().... 
                //OR you can enstanitATE roommate outside of the if statement, set it to null, and if you get data back you can update that object with the properties returned 
            }
        }


        //Roommates by RoomId
        //public List<Roommate> GetRoommatesByRoomId(int roomId)
        //Roommate objects should have a Room property
        public List<Roommate> GetRoommatesByRoomId(int roomId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Firstname, Lastname, RentPortion, MoveInDate, RoomId FROM Roommate WHERE roomId = @roomId";
                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    // If we only expect a single row back from the database, we don't need a while loop.
                    // A list to hold the rooms we retrieve from the database.
                    List<Roommate> GetRoommatesByRoomId = new List<Roommate>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(reader.GetOrdinal("Id"));
                        string FirstnameValue = reader.GetString(reader.GetOrdinal("Firstname"));
                        string LastnameValue = reader.GetString(reader.GetOrdinal("Lastname"));
                        int RentPortionValue = reader.GetInt32(reader.GetOrdinal("RentPortion"));
                        DateTime MoveInDateValue = reader.GetDateTime(reader.GetOrdinal("MoveInDate"));
                        int RoomId = reader.GetOrdinal("RoomId");
                        //initally being set as null inside the object so it does not need to be declared outside of it.
                        // Now let's create a new room object using the data from the database.
                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            Firstname = FirstnameValue,
                            Lastname = LastnameValue,
                            RentPortion = RentPortionValue,
                            MoveInDate = MoveInDateValue,
                            Room = null,
                        };
                        // ...and add that room object to our list.
                        GetRoommatesByRoomId.Add(roommate);
                    }
                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of rooms who whomever called this method.
                    return GetRoommatesByRoomId;
                }
            }
        }

        //Insert new Roommate
        public void Insert(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    
                    // These SQL parameters are annoying. Why can't we use string interpolation?
                    // ... sql injection attacks!!!
                    cmd.CommandText = @"INSERT INTO Roommate (FirstName, LastName, RentPortion, MoveInDate, RoomId) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@FirstName, @LastName, @RentPortion, @MoveInDate, @RoomId)";
                    cmd.Parameters.AddWithValue("@FirstName", roommate.Firstname);
                    cmd.Parameters.AddWithValue("@LastName", roommate.Lastname);
                    cmd.Parameters.AddWithValue("@RentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@MoveInDate", roommate.MoveInDate);

                    //RoomId = handling Room Class that is used as a property inside of roommate, 
                    //had to assign a variable equal to a room id (in program.cs) and then i could use that as the room object to be passed through
                    cmd.Parameters.AddWithValue("@RoomId",roommate.Room.Id);


                    int id = (int)cmd.ExecuteScalar();
                    //The cmd.ExecuteScalar method does two things: First, it executes the SQL command against the database. 
                    //Then it looks at the first thing that the database sends back (in our case this is just the Id it created for the room) and returns it.
                    // ....room.id declared here now that the result has been returned (similar to seeing the result when using .then statements
                    //with the rresult returned we have the Id that the SQL server assigned
                    roommate.Id = id;



                }
            }
        }


        //Update Roommate Entry

        
        public void Update(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Roommate
                                    SET FirstName = @FirstName,
                                        LastName = @LastName,
                                        RentPortion = @RentPortion,
                                        MoveInDate = @MoveInDate,
                                        RoomId = @RoomId
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@FirstName", roommate.Firstname);
                    cmd.Parameters.AddWithValue("@LastName", roommate.Lastname);
                    cmd.Parameters.AddWithValue("@RentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@MoveInDate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@RoomId", roommate.Room.Id);
                    cmd.Parameters.AddWithValue("@id", roommate.Id);

                    cmd.ExecuteNonQuery();
                    //We use this method when we want to execute a SQL command, but we don't expect anything back from the database.
                }
            }
        }


        /// <summary>
        ///  Delete the roommate with the given id
        /// </summary>
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }



    }
}

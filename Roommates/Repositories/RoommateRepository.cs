using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Roommates.Repositories
{
    ///<summary>
    ///  This class is responsible for interacting with Roommate data.
    ///  It inherits from the BaseRepository class so that it can use the BaseRepository's Connection property
    /// </summary>
    class RoommateRepository : BaseRepository
    {
        /// <summary>
        ///  When new RoomRespository is instantiated, pass the connection string along to the BaseRepository
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
        ///  Returns a single room with the given id.
        /// </summary>
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Firstname, Lastname, RentPortion, MoveInDate, RoomId FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    // If we only expect a single row back from the database, we don't need a while loop.
                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("Firstname")),
                            Lastname = reader.GetString(reader.GetOrdinal("Lastname")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),

                         
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = null,
                    };
                    }

                    reader.Close();

                    return roommate;
                }
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

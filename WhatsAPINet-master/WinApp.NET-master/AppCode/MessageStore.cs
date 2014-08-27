using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SQLite;

namespace WinAppNET.AppCode
{
    class MessageStore
    {
        public const string ConnectionString = "Data Source=data/sqlite/messages.db3";

        public static void CheckTable()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (DbConnection cnn = fact.CreateConnection())
            {
                cnn.ConnectionString = MessageStore.ConnectionString;
                cnn.Open();
                DbCommand cmd = cnn.CreateCommand();
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS
'Messages' (
'id' INTEGER PRIMARY KEY,
'jid' VARCHAR(64),
'author' VARCHAR(64),
'from_me' INTEGER,
'read' INTEGER,
'data' TEXT,
'type' VARCHAR(64),
'preview' TEXT,
'timestamp' VARCHAR(64)
)";
                cmd.ExecuteNonQuery();
            }
        }

        public static WappMessage[] GetAllMessagesForContact(string jid)
        {
            List<WappMessage> messages = new List<WappMessage>();

            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (DbConnection cnn = fact.CreateConnection())
            {
                cnn.ConnectionString = MessageStore.ConnectionString;
                cnn.Open();
                DbCommand cmd = cnn.CreateCommand();
                cmd = cnn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Messages where jid = @jid";
                cmd.Parameters.Add(new SQLiteParameter("@jid", jid));
                DbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = Int32.Parse(reader["id"].ToString());
                    string author = string.Empty;
                    try
                    {
                        if (reader["author"] != null)
                        {
                            author = reader["author"].ToString();
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    bool from_me = (Int32.Parse(reader["from_me"].ToString()) == 1 ? true : false);
                    string data = (string)reader["data"];
                    string type;
                    try
                    {
                        type = (string)reader["type"];
                    }catch(Exception)
                    {
                        type = string.Empty;
                    }
                    string preview;
                    try
                    {
                        preview = (string)reader["preview"];
                    }
                    catch (Exception)
                    {
                        preview = string.Empty;
                    }
                    DateTime timestamp = DateTime.Parse(reader["timestamp"].ToString());
                    WappMessage message = new WappMessage(id, data, from_me, jid, timestamp, type, preview);
                    if (!String.IsNullOrEmpty(author))
                    {
                        message.author = author;
                    }
                    messages.Add(message);
                }
            }

            return messages.ToArray();
        }

        public static void AddMessage(WappMessage message)
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (DbConnection cnn = fact.CreateConnection())
            {
                cnn.ConnectionString = MessageStore.ConnectionString;
                cnn.Open();
                DbCommand cmd = cnn.CreateCommand();
                cmd.CommandText = @"INSERT INTO
'Messages' 
(
'jid',
'author',
'from_me',
'data',
'timestamp',
'type',
'preview'
)
VALUES (
@jid,
@author,
@from_me,
@data,
@timestamp,
@type,
@preview
)";
                cmd.Parameters.Add(new SQLiteParameter("@jid", message.jid));
                cmd.Parameters.Add(new SQLiteParameter("@author", message.author));
                cmd.Parameters.Add(new SQLiteParameter("@from_me", (message.from_me ? "1" : "0")));
                cmd.Parameters.Add(new SQLiteParameter("@data", message.data));
                cmd.Parameters.Add(new SQLiteParameter("@timestamp", message.timestamp.ToString()));
                cmd.Parameters.Add(new SQLiteParameter("@type", message.type));
                cmd.Parameters.Add(new SQLiteParameter("@preview", message.preview));
                cmd.ExecuteNonQuery();
            }
        }
    }
}

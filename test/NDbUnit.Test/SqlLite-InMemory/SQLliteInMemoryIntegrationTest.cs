﻿/*
 *
 * NDbUnit
 * Copyright (C) 2005 - 2015
 * https://github.com/fubar-coder/NDbUnit
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using NDbUnit.Core.SqlLite;
using System.Data.SQLite;
using System.Data;
using System.Data.Common;
using System.IO;
using NDbUnit.Core;
using System.Diagnostics;
using NUnit.Framework;

namespace NDbUnit.Test.SqlLite_InMemory
{
    [Category(TestCategories.SqliteTests)]
    [TestFixture]
    public class SQLliteInMemoryIntegrationTest
    {
        private SQLiteConnection _connection;

        [TestFixtureSetUp]
        public void _TestFixtureSetUp()
        {
            _connection = new SQLiteConnection(DbConnections.SqlLiteInMemConnectionString);
            ExecuteSchemaCreationScript();
        }

        [Test]
        public void Can_Get_Data_From_In_Memory_Instance()
        {
            var database = new SqlLiteDbUnitTest(_connection);

            database.ReadXmlSchema(XmlTestFiles.Sqlite.XmlSchemaFile);
            database.ReadXml(XmlTestFiles.Sqlite.XmlFile);

            database.PerformDbOperation(DbOperationFlag.CleanInsertIdentity);

            var command = _connection.CreateCommand();
            command.CommandText = "Select * from [Role]";

            var results = command.ExecuteReader();
            
            Assert.IsTrue(results.HasRows);

            int recordCount = 0;

            while (results.Read())
            {
                recordCount++;
                Debug.WriteLine(results.GetString(1));
            }

            Assert.AreEqual(2, recordCount);

        }

        private void ExecuteSchemaCreationScript()
        {
            DbCommand command = _connection.CreateCommand();
            command.CommandText = ReadTextFromFile(@"scripts\sqlite-testdb-create.sql");

            using (new OpenConnectionGuard(_connection))
            {
                command.ExecuteNonQuery();

                command.CommandText = "Select * from Role";
                command.ExecuteReader();
            }
        }

        private string ReadTextFromFile(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                return sr.ReadToEnd();
            }
        }

    }
}

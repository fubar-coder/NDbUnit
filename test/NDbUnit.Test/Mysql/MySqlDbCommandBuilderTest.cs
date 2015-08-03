﻿/*
 *
 * NDbUnit
 * Copyright (C) 2005 - 2015
 * https://github.com/NDbUnit/NDbUnit
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

using System.Collections.Generic;
using MySql.Data.MySqlClient;
using NDbUnit.Core;
using NDbUnit.Core.MySqlClient;
using NUnit.Framework;

namespace NDbUnit.Test.SqlClient
{
    [Category(TestCategories.MySqlTests)]
    [TestFixture]
    class MySqlDbCommandBuilderTest : NDbUnit.Test.Common.DbCommandBuilderTestBase
    {
        public override IList<string> ExpectedDataSetTableNames
        {
            get
            {
                return new List<string>()
                {
                    "Role", "User", "UserRole" 
                };
            }
        }

        public override IList<string> ExpectedDeleteAllCommands
        {
            get
            {
                return new List<string>()
                {
                    "DELETE FROM `Role`",
                    "DELETE FROM `User`",
                    "DELETE FROM `UserRole`"
                };
            }
        }

        public override IList<string> ExpectedDeleteCommands
        {
            get
            {
                return new List<string>()
                {
                    "DELETE FROM `Role` WHERE `ID`=?p1",
                    "DELETE FROM `User` WHERE `ID`=?p1",
                    "DELETE FROM `UserRole` WHERE `UserID`=?p1 AND `RoleID`=?p2"
                };
            }
        }

        public override IList<string> ExpectedInsertCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO `Role`(`ID`, `Name`, `Description`) VALUES(?p1, ?p2, ?p3)",
                    "INSERT INTO `User`(`ID`, `FirstName`, `LastName`, `Age`, `SupervisorID`) VALUES(?p1, ?p2, ?p3, ?p4, ?p5)",
                    "INSERT INTO `UserRole`(`UserID`, `RoleID`) VALUES(?p1, ?p2)"
                };

            }
        }

        public override IList<string> ExpectedInsertIdentityCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO `Role`(`ID`, `Name`, `Description`) VALUES(?p1, ?p2, ?p3)",
                    "INSERT INTO `User`(`ID`, `FirstName`, `LastName`, `Age`, `SupervisorID`) VALUES(?p1, ?p2, ?p3, ?p4, ?p5)",
                    "INSERT INTO `UserRole`(`UserID`, `RoleID`) VALUES(?p1, ?p2)"
                };
            }
        }

        public override IList<string> ExpectedSelectCommands
        {
            get
            {
                return new List<string>()
                {
                    "SELECT `ID`, `Name`, `Description` FROM `Role`",
                    "SELECT `ID`, `FirstName`, `LastName`, `Age`, `SupervisorID` FROM `User`",
                    "SELECT `UserID`, `RoleID` FROM `UserRole`"
                };
            }
        }

        public override IList<string> ExpectedUpdateCommands
        {
            get
            {
                return new List<string>()
                {
                    "UPDATE `Role` SET `Name`=?p2, `Description`=?p3 WHERE `ID`=?p1",
                    "UPDATE `User` SET `FirstName`=?p2, `LastName`=?p3, `Age`=?p4, `SupervisorID`=?p5 WHERE `ID`=?p1",
                    "UPDATE `UserRole` SET `UserID`=?p2, `RoleID`=?p4 WHERE `UserID`=?p1 AND `RoleID`=?p3"
                };
            }
        }

        protected override IDisposableDbCommandBuilder GetDbCommandBuilder()
        {
            var connectionManager = new DbConnectionManager<MySqlConnection>(DbConnections.MySqlConnectionString);
            return new DisposableDbCommandBuilder<MySqlConnection>(connectionManager, new MySqlDbCommandBuilder(connectionManager));
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.MySql.XmlSchemaFile;
        }

    }
}

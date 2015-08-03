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

using System;
using System.Collections.Generic;
using NDbUnit.Core;
using System.Data;
using System.Data.Common;

using NUnit.Framework;

namespace NDbUnit.Test.Common
{
    public abstract class DbCommandBuilderTestBase
    {
        private const int EXPECTED_COUNT_OF_COMMANDS = 3;

        protected IDisposableDbCommandBuilder _commandBuilder;

        public abstract IList<string> ExpectedDataSetTableNames { get; }

        public abstract IList<string> ExpectedDeleteAllCommands { get; }

        public abstract IList<string> ExpectedDeleteCommands { get; }

        public abstract IList<string> ExpectedInsertCommands { get; }

        public abstract IList<string> ExpectedInsertIdentityCommands { get; }

        public abstract IList<string> ExpectedSelectCommands { get; }

        public abstract IList<string> ExpectedUpdateCommands { get; }

        [SetUp]
        public void _SetUp()
        {
            _commandBuilder = GetDbCommandBuilder();
            ExecuteSchemaCreationScript();
            _commandBuilder.BuildCommands(GetXmlSchemaFilename());

        }

        [TearDown]
        public void TearDown()
        {
            _commandBuilder.Dispose();
        }

        [Test]
        public void GetDeleteAllCommand_Creates_Correct_SQL_Commands()
        {
            IList<string> commandList = new List<string>();

            DataSet ds = _commandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                using (DbCommand dbCommand = _commandBuilder.GetDeleteAllCommand(null, dataTable.TableName))
                {
                    commandList.Add(dbCommand.CommandText);

                    Console.WriteLine("Table '" + dataTable.TableName + "' delete all command");
                    Console.WriteLine("\t" + dbCommand.CommandText);
                }
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, commandList.Count, string.Format("Should be {0} commands", EXPECTED_COUNT_OF_COMMANDS));
            Assert.That(ExpectedDeleteAllCommands, Is.EquivalentTo(commandList));
        }

        [Test]
        public void GetDeleteCommand_Creates_Correct_SQL_Commands()
        {
            IList<string> commandList = new List<string>();

            DataSet ds = _commandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                using (DbCommand dbCommand = _commandBuilder.GetDeleteCommand(null, dataTable.TableName))
                {
                    commandList.Add(dbCommand.CommandText);

                    Console.WriteLine("Table '" + dataTable.TableName + "' delete command");
                    Console.WriteLine("\t" + dbCommand.CommandText);
                }
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, commandList.Count, string.Format("Should be {0} commands", EXPECTED_COUNT_OF_COMMANDS));
            Assert.That(ExpectedDeleteCommands, Is.EquivalentTo(commandList));
        }

        [Test]
        public void GetInsertCommand_Creates_Correct_SQL_Commands()
        {
            DataSet ds = _commandBuilder.GetSchema();
            IList<string> commandList = new List<string>();

            foreach (DataTable dataTable in ds.Tables)
            {
                using (DbCommand dbCommand = _commandBuilder.GetInsertCommand(null, dataTable.TableName))
                {
                    commandList.Add(dbCommand.CommandText);

                    Console.WriteLine("Table '" + dataTable.TableName + "' insert command");
                    Console.WriteLine("\t" + dbCommand.CommandText);
                }
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, commandList.Count, string.Format("Should be {0} commands", EXPECTED_COUNT_OF_COMMANDS));
            Assert.That(ExpectedInsertCommands, Is.EquivalentTo(commandList));
        }

        [Test]
        public void GetInsertIdentityCommand_Creates_Correct_SQL_Commands()
        {
            IList<string> commandList = new List<string>();
            DataSet ds = _commandBuilder.GetSchema();

            foreach (DataTable dataTable in ds.Tables)
            {
                using (DbCommand dbCommand = _commandBuilder.GetInsertIdentityCommand(null, dataTable.TableName))
                {
                    commandList.Add(dbCommand.CommandText);

                    Console.WriteLine("Table '" + dataTable.TableName + "' insert identity command");
                    Console.WriteLine("\t" + dbCommand.CommandText);
                }
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, commandList.Count, string.Format("Should be {0} commands", EXPECTED_COUNT_OF_COMMANDS));
            Assert.That(ExpectedInsertIdentityCommands, Is.EquivalentTo(commandList));
        }

        [Test]
        public void GetSchema_Contains_Proper_Tables()
        {
            using (IDisposableDbCommandBuilder builder = GetDbCommandBuilder())
            {
                builder.BuildCommands(GetXmlSchemaFilename());
                DataSet schema = builder.GetSchema();

                IList<string> schemaTables = new List<string>();

                foreach (DataTable dataTable in schema.Tables)
                {
                    schemaTables.Add(dataTable.TableName);

                    Console.WriteLine("Table '" + dataTable.TableName + "' found in dataset");
                }

                Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, schema.Tables.Count, string.Format("Should be {0} Tables in dataset", EXPECTED_COUNT_OF_COMMANDS));
                Assert.That(ExpectedDataSetTableNames, Is.EquivalentTo(schemaTables));
            }
        }

        [Test]
        public void GetSchema_Throws_NDbUnit_Exception_When_Not_Initialized()
        {
            using (IDisposableDbCommandBuilder builder = GetDbCommandBuilder())
            {
                try
                {
                    builder.GetSchema();
                    Assert.Fail("Expected Exception of type NDbUnitException not thrown!");
                }
                catch (NDbUnitException ex)
                {
                    Assert.IsNotNull(ex);
                }
            }
        }

        [Test]
        public void GetSelectCommand_Creates_Correct_SQL_Commands()
        {
            IList<string> commandList = new List<string>();
            DataSet ds = _commandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                using (DbCommand dbCommand = _commandBuilder.GetSelectCommand(null, dataTable.TableName))
                {
                    commandList.Add(dbCommand.CommandText);

                    Console.WriteLine("Table '" + dataTable.TableName + "' select command");
                    Console.WriteLine("\t" + dbCommand.CommandText);
                }
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, commandList.Count, string.Format("Should be {0} commands", EXPECTED_COUNT_OF_COMMANDS));
            Assert.That(ExpectedSelectCommands, Is.EquivalentTo(commandList));
        }

        [Test]
        public void GetUpdateCommand_Creates_Correct_SQL_Commands()
        {
            IList<string> commandList = new List<string>();

            DataSet ds = _commandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                using (DbCommand dbCommand = _commandBuilder.GetUpdateCommand(null, dataTable.TableName))
                {
                    commandList.Add(dbCommand.CommandText);

                    Console.WriteLine("Table '" + dataTable.TableName + "' update command");
                    Console.WriteLine("\t" + dbCommand.CommandText);
                }
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, commandList.Count, string.Format("Should be {0} commands", EXPECTED_COUNT_OF_COMMANDS));
            Assert.That(ExpectedUpdateCommands, Is.EquivalentTo(commandList));
        }

        protected abstract IDisposableDbCommandBuilder GetDbCommandBuilder();

        protected abstract string GetXmlSchemaFilename();

        protected virtual void ExecuteSchemaCreationScript()
        {
            //default behavior performs no action, override in derived class as needed
        }

    }
}

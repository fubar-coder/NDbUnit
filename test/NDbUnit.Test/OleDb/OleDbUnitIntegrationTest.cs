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

using System;
using System.Collections.Generic;
using System.Text;
using NDbUnit.Core.SqlClient;
using NDbUnit.Core.OleDb;
using NUnit.Framework;

namespace NDbUnit.Test.SqlClient
{
    [Category(TestCategories.OleDbTests)]
    public class OleDbUnitIntegrationTest : IntegationTestBase
    {
        protected override NDbUnit.Core.INDbUnitTest GetNDbUnitTest()
        {
            return new OleDbUnitTest(DbConnections.OleDbConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.OleDb.XmlFile;
        }

        protected override string GetXmlModFilename()
        {
            return XmlTestFiles.OleDb.XmlModFile;
        }

        protected override string GetXmlRefreshFilename()
        {
            return XmlTestFiles.OleDb.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OleDb.XmlSchemaFile;
        }

    }
}

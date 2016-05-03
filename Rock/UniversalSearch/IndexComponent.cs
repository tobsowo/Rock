﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.Linq;

using Rock.Attribute;
using Rock.Extension;
using Rock.UniversalSearch.IndexModels;

namespace Rock.UniversalSearch
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class IndexComponent : Component
    {
        public abstract bool IsConnected { get; }

        public abstract string IndexLocation { get; }

        public abstract string IndexName { get; }

        public abstract void IndexDocument( string typeName, IndexModelBase document );
    }
}
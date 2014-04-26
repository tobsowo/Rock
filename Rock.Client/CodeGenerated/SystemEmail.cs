//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright 2013 by the Spark Development Network
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


namespace Rock.Client
{
    /// <summary>
    /// Data Transfer Object for SystemEmail object
    /// </summary>
    public partial class SystemEmail
    {
        /// <summary />
        public bool IsSystem { get; set; }

        /// <summary />
        public string Category { get; set; }

        /// <summary />
        public string Title { get; set; }

        /// <summary />
        public string From { get; set; }

        /// <summary />
        public string FromName { get; set; }

        /// <summary />
        public string To { get; set; }

        /// <summary />
        public string Cc { get; set; }

        /// <summary />
        public string Bcc { get; set; }

        /// <summary />
        public string Subject { get; set; }

        /// <summary />
        public string Body { get; set; }

        /// <summary />
        public DateTime? CreatedDateTime { get; set; }

        /// <summary />
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary />
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary />
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public string ForeignId { get; set; }

    }
}

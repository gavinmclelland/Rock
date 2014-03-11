﻿// <copyright>
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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using Rock.Extension;

namespace Rock.Reporting
{
    /// <summary>
    /// MEF Container class for data filters
    /// </summary>
    public class DataFilterContainer : Container<DataFilterComponent, IComponentData>
    {
        private static DataFilterContainer instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>

        public static DataFilterContainer Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = new DataFilterContainer();
                }
                return instance;
            }
        }

        private DataFilterContainer()
        {
            Refresh();
        }

        /// <summary>
        /// Gets a list of entity type names that have Data Filter components
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAvailableFilteredEntityTypeNames()
        {
            var entityTypeNames = new List<string>();

            foreach ( var serviceEntry in Instance.Components )
            {
                var component = serviceEntry.Value.Value;
                if ( !entityTypeNames.Contains( component.AppliesToEntityType ) )
                {
                    entityTypeNames.Add( component.AppliesToEntityType );
                }
            }

            return entityTypeNames;
        }

        /// <summary>
        /// Gets the component with the matching Entity Type Name
        /// </summary>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <returns></returns>
        public static DataFilterComponent GetComponent( string entityTypeName )
        {
            foreach ( var serviceEntry in Instance.Components )
            {
                var component = serviceEntry.Value.Value;
                if ( component.TypeName == entityTypeName )
                {
                    return component;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the components that are for filtering a given entity type name
        /// </summary>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <returns></returns>
        public static List<DataFilterComponent> GetComponentsByFilteredEntityName( string entityTypeName )
        {
            return Instance.Components
                .Where( c => 
                    c.Value.Value.AppliesToEntityType == entityTypeName ||
                    c.Value.Value.AppliesToEntityType == string.Empty )
                .Select( c => c.Value.Value )
                .OrderBy( c => c.Order)
                .ToList();
        }

        // MEF Import Definition
#pragma warning disable
        [ImportMany( typeof( DataFilterComponent ) )]
        protected override IEnumerable<Lazy<DataFilterComponent, IComponentData>> MEFComponents { get; set; }
#pragma warning restore

    }
}
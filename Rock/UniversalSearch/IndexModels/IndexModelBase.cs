﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Rock.Attribute;

namespace Rock.UniversalSearch.IndexModels
{
    public class IndexModelBase : DynamicObject
    {
        private Dictionary<string, object> _members = new Dictionary<string, object>();
        object Instance;
        Type InstanceType;

        PropertyInfo[] InstancePropertyInfo
        {
            get
            {
                if ( _InstancePropertyInfo == null && Instance != null )
                    _InstancePropertyInfo = Instance.GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly );
                return _InstancePropertyInfo;
            }
        }
        PropertyInfo[] _InstancePropertyInfo;


        public int Id { get; set; }


        public IndexModelBase()
        {
            Instance = this;
            InstanceType = this.GetType();
        }

        protected static void AddIndexableAttributes( IndexModelBase indexModel, IHasAttributes sourceModel )
        {
            sourceModel.LoadAttributes();

            foreach ( var attribute in sourceModel.Attributes )
            {
                indexModel[attribute.Key] = sourceModel.AttributeValues[attribute.Key].Value;
            }
        }


        public override bool TryGetMember( GetMemberBinder binder, out object result )
        {
            result = null;

            // first check the dictionary for member
            if ( _members.Keys.Contains( binder.Name ) )
            {
                result = _members[binder.Name];
                return true;
            }


            // next check for public properties via Reflection
            try
            {
                return GetProperty( Instance, binder.Name, out result );
            }
            catch { }
            

            // failed to retrieve a property
            result = null;
            return false;
        }

        public override bool TrySetMember( SetMemberBinder binder, object value )
        {

            // first check to see if there's a native property to set
            if ( Instance != null )
            {
                try
                {
                    bool result = SetProperty( this, binder.Name, value );
                    if ( result )
                        return true;
                }
                catch { }
            }

            // no match - set or add to dictionary
            _members[binder.Name] = value;
            return true;
        }

        protected bool GetProperty( object instance, string name, out object result )
        {
            if ( instance == null )
                instance = this;

            var miArray = InstanceType.GetMember( name, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance );
            if ( miArray != null && miArray.Length > 0 )
            {
                var mi = miArray[0];
                if ( mi.MemberType == MemberTypes.Property )
                {
                    result = ((PropertyInfo)mi).GetValue( instance, null );
                    return true;
                }
            }

            result = null;
            return false;
        }

        protected bool SetProperty( object instance, string name, object value )
        {
            if ( instance == null )
                instance = this;

            var miArray = InstanceType.GetMember( name, BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance );
            if ( miArray != null && miArray.Length > 0 )
            {
                var mi = miArray[0];
                if ( mi.MemberType == MemberTypes.Property )
                {
                    ((PropertyInfo)mi).SetValue( Instance, value, null );
                    return true;
                }
            }
            return false;
        }

        public object this[string key]
        {
            get
            {
                try
                {
                    // try to get from properties collection first
                    return _members[key];
                }
                catch ( KeyNotFoundException ex )
                {
                    // try reflection on instanceType
                    object result = null;
                    if ( GetProperty( Instance, key, out result ) )
                        return result;

                    // nope doesn't exist
                    throw;
                }
            }
            set
            {
                if ( _members.ContainsKey( key ) )
                {
                    _members[key] = value;
                    return;
                }

                // check instance for existance of type first
                var miArray = InstanceType.GetMember( key, BindingFlags.Public | BindingFlags.GetProperty );
                if ( miArray != null && miArray.Length > 0 )
                    SetProperty( Instance, key, value );
                else
                    _members[key] = value;
            }
        }

        public IEnumerable<KeyValuePair<string, object>> GetProperties( bool includeInstanceProperties = false )
        {
            if ( includeInstanceProperties && Instance != null )
            {
                foreach ( var prop in this.InstancePropertyInfo )
                    yield return new KeyValuePair<string, object>( prop.Name, prop.GetValue( Instance, null ) );
            }

            foreach ( var key in this._members.Keys )
                yield return new KeyValuePair<string, object>( key, this._members[key] );

        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            List<string> propertyNames = new List<string>();
            foreach ( var prop in this.InstancePropertyInfo )
            {
                propertyNames.Add( prop.Name );
            }

            foreach ( var key in this._members.Keys )
            {
                propertyNames.Add( key );
            }

            return propertyNames;
        }

        public bool Contains( KeyValuePair<string, object> item, bool includeInstanceProperties = false )
        {
            bool res = _members.ContainsKey( item.Key );
            if ( res )
                return true;

            if ( includeInstanceProperties && Instance != null )
            {
                foreach ( var prop in this.InstancePropertyInfo )
                {
                    if ( prop.Name == item.Key )
                        return true;
                }
            }

            return false;
        }
    }
}
﻿// Object store class.
// Copyright (C) 2009-2010 Lex Li
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// SNMP object store, who holds all implemented SNMP objects in the agent.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    public class ObjectStore
    {
        /// <summary>The internal list of objects holding the data.</summary>
        protected readonly IList<ISnmpObject> List = new List<ISnmpObject>();

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="id">The object id.</param>
        /// <returns></returns>
        public virtual ScalarObject GetObject(ObjectIdentifier id)
        {
            foreach (var o in List)
            {
                var result = o.MatchGet(id);
                if (result != null)
                    return result;
            }

            return null;
        }

        /// <summary>
        /// Gets the next object.
        /// </summary>
        /// <param name="id">The object id.</param>
        /// <returns></returns>
        public virtual ScalarObject GetNextObject(ObjectIdentifier id)
        {
            foreach (var o in List)
            {
                var result = o.MatchGetNext(id);
                if (result != null)
                    return result;
            }

            return null;
        }

        /// <summary>
        /// Adds the specified <see cref="ISnmpObject"/>.
        /// </summary>
        /// <param name="newObject">The object.</param>
        public virtual void Add(ISnmpObject newObject)
        {
            List.Add(newObject);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using XstReader.ElementProperties;

namespace XstReader.MsgKit
{
    /// <summary>
    ///     Used to hold exactly one named property tag
    /// </summary>
    internal class NamedPropertyTag
    {
        #region Properties
        /// <summary>
        ///     The 2 byte identifier
        /// </summary>
        public ushort Id { get; }

        /// <summary>
        ///     The name of the property
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The <see cref="Guid" />
        /// </summary>
        public Guid Guid { get; }

        /// <summary>
        ///     The 2 byte <see cref="PropertyType" />
        /// </summary>
        public PropertyType Type { get; }
        #endregion

        #region Constructor
        /// <summary>
        ///     Creates this object and sets all its properties
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="name">The name of the property</param>
        /// <param name="guid">The property <see cref="Guid" /></param>
        /// <param name="type">The <see cref="PropertyType" /></param>
        internal NamedPropertyTag(ushort id, string name, Guid guid, PropertyType type)
        {
            Id = id;
            Name = name;
            Guid = guid;
            Type = type;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AutoMapper.Internal;

namespace NeuroMedia.Application.Common.Helpers
{
    public static class MappingHelper<T>
    {
        public static T MapTopLevel(object source, T destination)
        {
            var sourceProperties = source.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                foreach (var destinationProperty in destinationProperties)
                {
                    if (sourceProperty.Name == destinationProperty.Name && sourceProperty.GetMemberType() == destinationProperty.GetMemberType())
                    {
                        if (destinationProperty.CanBeSet())
                        {
                            var value = sourceProperty.GetValue(source, null);
                            destinationProperty.SetValue(destination, value, null);
                        }
                    }
                }
            }

            return destination;
        }
    }
}

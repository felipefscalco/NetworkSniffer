using Application.Enums;
using NetworkCommon.Models;
using System;
using System.Collections.Generic;

namespace Application.Validations
{
    public class FilterValidation
    {
        public List<ProtocolFilter> ProtocolFilters { get; set; }

        public FilterValidation()
        {
            ProtocolFilters = new List<ProtocolFilter>();
        }

        internal bool IsFilterValid(string filterText)
        {
            ProtocolFilters.Clear();
            var isValid = false;

            string[] stringSeparators = new string[] { " && " };  
            var filters = filterText.Split(stringSeparators, StringSplitOptions.None);
            foreach (var filter in filters)
            {
                if (filter.Equals("protocol udp", StringComparison.InvariantCultureIgnoreCase))
                {
                    ProtocolFilters.Add(ProtocolFilter.UDP);
                    isValid = true;
                }
                else if (filter.Equals("protocol tcp", StringComparison.InvariantCultureIgnoreCase))
                {
                    ProtocolFilters.Add(ProtocolFilter.TCP);
                    isValid = true;
                }
                else if (filter.Equals("protocol icmpv6", StringComparison.InvariantCultureIgnoreCase))
                {
                    ProtocolFilters.Add(ProtocolFilter.ICMPV6);
                    isValid = true;
                }
                else if (filter.Equals("protocol igmp", StringComparison.InvariantCultureIgnoreCase))
                {
                    ProtocolFilters.Add(ProtocolFilter.IGMP);
                    isValid = true;
                }
            }

            return isValid;        
        }

        internal bool ShouldAddPackageToList(Packet packet)
        {
            ProtocolFilters.ForEach(p => p.ToString().Equals(packet.Protocol));

            foreach (var protocol in ProtocolFilters)
            {
                if (packet.Protocol.Equals(protocol.ToString()))
                    return true;
            }

            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IapdLabWork4
{
    class DeviceInformation
    {
        private string name, description;
        private string manufacturer;
        private Dictionary<string, string> parameters;

        public void setName(string name)
        {
            this.name = name;
        }

        public void setDescription(string description)
        {
            this.description = description;
        }

        public void setManufacturer(string m)
        {
            this.manufacturer = m;
        }

        public void setParameters(Dictionary<string, string> parameters)
        {
            this.parameters = parameters;
        }

        public string getName()
        {
            return name;
        }

        public string getDesription()
        {
            return description;
        }

        public string getManufacturer()
        {
            return manufacturer;
        }

        public Dictionary<string, string> getParameters()
        {
            return parameters;
        }
    }
}

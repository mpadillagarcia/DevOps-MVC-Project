using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Exchange.UIT.Alertas
{
    public class AlertaCriptomonedasTestDataGeneratorBasicFlow : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
    {
        new object[] {"2", "2", "30/12/2022" }

    };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
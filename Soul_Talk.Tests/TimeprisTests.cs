using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Soul_Talk.Models;
using Soul_Talk.Models.Forretningslogik;

namespace Soul_Talk.Tests
{
    [TestClass]
    public class TimeprisTests
    {
        [TestMethod]
        public void HentTimepris_OffentligInstitution_Fysisk_Giver550()
        {
            // Arrange (forbered data)
            Timepris beregner = new Timepris();

            Institution inst = new Institution();
            inst.Type = InstitutionType.Offentlig;

            Kunde kunde = new Kunde();
            kunde.Institution = inst;

            // Act (kald metoden vi vil teste)
            decimal resultat = beregner.HentTimepris(kunde, true); // true = fysisk

            // Assert (sikre at resultatet er som forventet)
            Assert.AreEqual(550m, resultat);
        }
    }
}

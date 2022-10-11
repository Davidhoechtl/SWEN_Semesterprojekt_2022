using MTCG.Logic.Models.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Tests
{
    internal class BattleLogicTests
    {
        [Test]
        public void Test_DragonVsGoblin()
        {
            Dragon dragon = new Dragon { Damage = 10 };
            Goblin goblin = new Goblin { Damage = 10 };

            Card result = dragon.BattleAgainst(goblin);
            Card result2 = goblin.BattleAgainst(dragon);

            Assert.That(dragon == result);
            Assert.That(dragon == result2);
        }

        [Test]
        public void Test_WizardVsOrk()
        {
            Wizzard wizzard = new Wizzard { Damage = 10 };
            Ork ork = new Ork { Damage = 10 };

            Card result = wizzard.BattleAgainst(ork);
            Card result2 = ork.BattleAgainst(wizzard);

            Assert.That(wizzard == result);
            Assert.That(wizzard == result2);
        }

        [Test]
        public void Test_KnightsVsWaterspells()
        {
            Knight knight = new Knight { Damage = 10 };
            WaterSpell waterSpell = new WaterSpell { Damage = 10 };

            Card result = knight.BattleAgainst(waterSpell);
            Card result2 = waterSpell.BattleAgainst(knight);

            Assert.That(waterSpell == result);
            Assert.That(waterSpell == result2);
        }

        [Test]
        public void Test_KrakenVsSpells()
        {
            Kraken kraken = new() { Damage = 10 };
            WaterSpell waterSpell = new() { Damage = 10 };

            Card result = kraken.BattleAgainst(waterSpell);
            Card result2 = waterSpell.BattleAgainst(kraken);

            Assert.That(kraken == result);
            Assert.That(kraken == result2);
        }

        [Test]
        public void Test_FireElveVsDragon()
        {
            FireElve elve = new() { Damage = 10 };
            Dragon dragon = new() { Damage = 10 };

            Card result = elve.BattleAgainst(dragon);
            Card result2 = dragon.BattleAgainst(elve);

            Assert.That(elve == result);
            Assert.That(elve == result2);
        }

        [Test]
        public void Test_FireSpellVsWaterSpell()
        {
            FireSpell fireSpell = new() { Damage = 10, ElementTyp = ElementTyp.Fire };
            WaterSpell waterSpell = new() { Damage = 10, ElementTyp = ElementTyp.Water };

            Card result = fireSpell.BattleAgainst(waterSpell);
            Card result2 = waterSpell.BattleAgainst(fireSpell);

            Assert.That(waterSpell == result);
            Assert.That(waterSpell == result2);
        }

        [Test]
        public void Test_FireSpellVsRegularSpell()
        {
            FireSpell fireSpell = new() { Damage = 10, ElementTyp = ElementTyp.Fire };
            RegularSpell regularSpell = new() { Damage = 10, ElementTyp = ElementTyp.Normal };

            Card result = fireSpell.BattleAgainst(regularSpell);
            Card result2 = regularSpell.BattleAgainst(fireSpell);

            Assert.That(fireSpell == result);
            Assert.That(fireSpell == result2);
        }

        [Test]
        public void Test_WaterSpellVsRegularSpell()
        {
            WaterSpell waterSpell = new() { Damage = 10, ElementTyp = ElementTyp.Water };
            RegularSpell regularSpell = new() { Damage = 10, ElementTyp = ElementTyp.Normal };

            Card result = waterSpell.BattleAgainst(regularSpell);
            Card result2 = regularSpell.BattleAgainst(waterSpell);

            Assert.That(regularSpell == result);
            Assert.That(regularSpell == result2);
        }

        [Test]
        public void Test_RegularSpellVsRegularSpell()
        {
            RegularSpell regularSpell = new() { Damage = 10, ElementTyp = ElementTyp.Normal };
            RegularSpell regularSpell2 = new() { Damage = 10, ElementTyp = ElementTyp.Normal };

            Card result = regularSpell.BattleAgainst(regularSpell2);
            Card result2 = regularSpell2.BattleAgainst(regularSpell);

            Assert.IsNull(result);
            Assert.IsNull(result2);
        }

        [Test]
        public void Test_FireSpellVsWaterMonster()
        {
            FireSpell fireSpell = new() { Damage = 10, ElementTyp = ElementTyp.Fire };
            Goblin waterGoblin = new() { Damage = 10, ElementTyp = ElementTyp.Water };

            Card result = fireSpell.BattleAgainst(waterGoblin);
            Card result2 = waterGoblin.BattleAgainst(fireSpell);

            Assert.That(waterGoblin == result);
            Assert.That(waterGoblin == result2);
        }
    }
}

using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Linq;

namespace HomeworkATM
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // изменения вносить было не сложно,
            // так как изначально переменные и названия методов были названы понятно,
            // в соответствии с их предназначением.
            // Сложностей с неочевидными связями между частями кода также не было,
            // так как были комментарии, поясняющие работу тех или иных частей кода

            var _card = new Card("1111111111111111", "Daria", "10.2025", "Sber", 15746);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"ИНФОРМАЦИЯ ПО КАРТЕ {_card.cardNumber}");
            Console.WriteLine(_card.ToString());
            Console.WriteLine("----------------------------------------");
            var _card2 = new Card("1110311411115001", "Daria", "10.2029", "Tinkoff", 0);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"ИНФОРМАЦИЯ ПО КАРТЕ {_card2.cardNumber}");
            Console.WriteLine(_card2.ToString());
            Console.WriteLine("----------------------------------------");

            
            var stack100 = new Stack<Banknote>();
            stack100.Push(new Banknote("100", "AB123456789"));
            stack100.Push(new Banknote("100", "AB176656789"));
            stack100.Push(new Banknote("100", "AB123454459"));
            stack100.Push(new Banknote("100", "AB002356789"));

            var stack10 = new Stack<Banknote>();
            stack10.Push(new Banknote("10", "AC123996789"));
            stack10.Push(new Banknote("10", "AC123000789"));
            stack10.Push(new Banknote("10", "AC133333789"));
            stack10.Push(new Banknote("10", "AC190900789"));

            var stack500 = new Stack<Banknote>();
            stack500.Push(new Banknote("500", "AD285644987"));
            stack500.Push(new Banknote("500", "AD111000789"));
            stack500.Push(new Banknote("500", "AD993333789"));
            stack500.Push(new Banknote("500", "AD000900789"));


            var dic = new Dictionary<string, Stack<Banknote>>();
            dic.Add("10", stack10);
            dic.Add("100", stack100);
            dic.Add("500", stack500);

            var transHistory = new List<string>();

            var _atm = new ATM("Sber", dic, transHistory, "74306199653196");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("ИНФОРМАЦИЯ ПРО ATM");
            Console.WriteLine(_atm.ToString());
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Общее кол-во купюр в банкомате: {_atm.CashAmount()}");
            Console.WriteLine("----------------------------------------");

            var dicAdd = new Dictionary<string, Banknote>();
            dicAdd.Add("10", new Banknote("10", "AB123456798"));
            dicAdd.Add("100", new Banknote("100", "XY987654321"));
            dicAdd.Add("231", new Banknote("100", "MN246813579"));

            var dicAdd2 = new Dictionary<string, Banknote>();
            dicAdd2.Add("100", new Banknote("100", "EF111122229"));
            dicAdd2.Add("500", new Banknote("500", "AB123456799"));

            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"ПОПОЛНЕНИЕ КАРТЫ {_card.cardNumber}:");
            Console.WriteLine("Сообщения от банкомата");
            _atm.ToppingUpCard(_card, dicAdd);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"ИНФОРМАЦИЯ ПО КАРТЕ {_card.cardNumber}");
            Console.WriteLine(_card.ToString());
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"ПОПОЛНЕНИЕ КАРТЫ {_card2.cardNumber}:");
            Console.WriteLine("Сообщения от банкомата");
            _atm.ToppingUpCard(_card2, dicAdd2);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"ИНФОРМАЦИЯ ПО КАРТЕ {_card2.cardNumber}");
            Console.WriteLine(_card2.ToString());
            Console.WriteLine("----------------------------------------");

            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"СНЯТИЕ С КАРТЫ {_card.cardNumber}:");
            Console.WriteLine("Сообщения от банкомата");
            _atm.WithdrawalCard(_card, 500);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"ИНФОРМАЦИЯ ПО КАРТЕ {_card.cardNumber}");
            Console.WriteLine(_card.ToString());
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"СНЯТИЕ С КАРТЫ {_card2.cardNumber}:");
            Console.WriteLine("Сообщения от банкомата");
            _atm.WithdrawalCard(_card2, 1500);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"ИНФОРМАЦИЯ ПО КАРТЕ {_card2.cardNumber}");
            Console.WriteLine(_card2.ToString());
            Console.WriteLine("----------------------------------------");

            //Console.WriteLine(NewCrypt("1428394874562349"));

            _atm.Pickup("1428394874562349");
            Console.WriteLine("----------------------------------------");
            _atm.Pickup("1428394874562349");


            Console.WriteLine("----------------------------------------");
            Console.WriteLine("ИНФОРМАЦИЯ ПРО ATM");
            Console.WriteLine(_atm.ToString());
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Общее кол-во купюр в банкомате: {_atm.CashAmount()}");
            Console.WriteLine("----------------------------------------");

        }
        /*
        //ПОПОЛНЕНИЕ
        public static void ToppingUpCard(Card card, Dictionary<string, Banknote> setting, ATM atm)
        {
            foreach (var key in setting.Keys)
            {
                if (!CheckIssuingBank(card, atm))
                {
                    Console.WriteLine("Банк-эмитент карты не совпадает с банком, владеющим банкоматом. Вводится комиссия 5%");
                    atm.transHistory.Add($"{card.cardNumber}: пополнение ({int.Parse(key) - 0.05 * int.Parse(key)} - комиссия 5%) => выполнено");
                    ChangingBalance(card, int.Parse(key) - 0.05 * int.Parse(key));
                    continue;
                }
                else if (!CheckSeries(setting[key].Series))
                {
                    Console.WriteLine("Банкнота не прошла проверку на подлинность!");
                    atm.transHistory.Add($"{card.cardNumber}: пополнение ({int.Parse(key)}) => error");
                    continue;
                }
                else if (!CheckendDate(card, atm))
                {
                    atm.transHistory.Add($"{card.cardNumber}: пополнение ({int.Parse(key)}) => error");
                    Console.WriteLine(new Exception("Истек срок действия карты"));
                    continue;
                }
                else if (!atm.ATMkass.ContainsKey(key))
                {
                    atm.transHistory.Add($"{card.cardNumber}: пополнение ({int.Parse(key)}) => error");
                    Console.WriteLine(new Exception($"Не найден данный номинал {key}"));
                    continue;
                }
                else
                {
                    atm.transHistory.Add($"{card.cardNumber}: пополнение ({int.Parse(key)}) => выполнено");
                    ChangingBalance(card, int.Parse(key));
                }
            }
        }

        // проверка серии банкноты
        static public bool CheckSeries(string series)
        {
            var flag = false;
            if ((Math.Abs(series[0] - series[1])-1) % 2 == 0)
            {
                flag = true;
            }
            var sum = 0;
            for (int i = 2; i < series.Length; i++)
            {
                sum += series[i] - '0';
            }
            if (sum % 2 != 0 && flag)
            {
                return true;
            }
            return false;
        }

        //СНЯТИЕ

        static public void WithdrawalCard(Card card, double sumRaw, ATM atm)
        {
            if (!CheckIssuingBank(card, atm) && sumRaw <= card.moneySum)
            {
                Console.WriteLine("Банк-эмитент карты не совпадает с банком, владеющим банкоматом. Вводится комиссия 5%");
                atm.transHistory.Add($"{card.cardNumber}: снятие ({sumRaw - 0.05 * sumRaw} - комиссия 5%) => выполнено");
                ChangingBalance(card, -(sumRaw - 0.05 * sumRaw));
                return;
            }
            else if (!CheckendDate(card, atm))
            {

                atm.transHistory.Add($"{card.cardNumber}: снятие ({sumRaw}) => error");
                Console.WriteLine(new Exception("Истек срок действия карты"));
                return;
            }
            else if (sumRaw > card.moneySum)
            {
                atm.transHistory.Add($"{card.cardNumber}: снятие ({sumRaw}) => error");
                Console.WriteLine(new Exception($"На счету недостаточно средств для снятия суммы {sumRaw}"));
                return;
            }
            else
            {
                atm.transHistory.Add($"{card.cardNumber}: снятие ({sumRaw}) => выполнено");
                ChangingBalance(card, -sumRaw);
                return;
            }
        }


        // изменение баланса карты
        public static void ChangingBalance(Card card, double addSum)
        {
            card.moneySum += addSum;
        }

        //проверка даты истечения действия карты
        public static bool CheckendDate(Card card, ATM atm)
        {
            if (int.Parse(card.endDate.Split('.')[1]) > int.Parse($"{DateTime.Now.Year}"))
            {
                return true;
            }
            else if (int.Parse(card.endDate.Split('.')[1]) == int.Parse($"{DateTime.Now.Year}"))
            {
                return int.Parse(card.endDate.Split('.')[0]) > int.Parse($"{DateTime.Now.Month}");
            }
            return false;
        }

        //проверка принадлежит ли банкомат данному банку
        public static bool CheckIssuingBank(Card card, ATM atm)
        {
            return card.issuingBank == atm.ownerATM;
        }


        //ИНКАССАЦИЯ
        static public void Pickup(ATM atm, string keyInc)
        {
            if (NewCrypt(keyInc) == atm.validKey)
            {
                var dicHashSet = new Dictionary<string, HashSet<string>>(); // 1 номинал = 1 множество
                foreach (var item in atm.ATMkass)
                {
                    foreach (var banknote in item.Value)
                    {
                        if (!dicHashSet.ContainsKey(item.Key))
                        {
                            dicHashSet[item.Key] = new HashSet<string>();
                        }
                        if (dicHashSet[item.Key].Contains(banknote.Series))
                        {
                            Console.WriteLine("В кассете была фальшивка. Найдена купюра с повторяющейся серией!");
                            return;
                        }
                        dicHashSet[item.Key].Add(banknote.Series);
                    }
                }
                var intersections = new HashSet<string>();
                
                foreach (var item in dicHashSet.Values)
                {
                    intersections.IntersectWith(item);
                }

                if (intersections.Count > 0)
                {
                    Console.WriteLine("Обнаружены фальшивые купюры по повторяющимся сериям:");
                    foreach (var item in intersections)
                    {
                        Console.WriteLine(item);
                    }
                }
                else
                {
                    Console.WriteLine("Фальшивые купюры не обнаружены.");
                }
                foreach (var item in atm.ATMkass)
                {
                    atm.ATMkass[item.Key].Clear();
                }
                Console.WriteLine("КАССЕТА ИЗВЛЕЧЕНА");
                atm.ShowTrans();
            }
            else
            {
                Console.WriteLine("КЛЮЧИ НЕ СОВПАДАЮТ! ПОЛИЦИЯ УЖЕ ЗА ДВЕРЬЮ!");
            }

        }

        // новый метод дешифрования
        static public string NewCrypt(string key)
        {
            var decryptKey = "";
            for (int i = 0; i < key.Length-2; i++)
            {
                var s = 0;
                s += int.Parse(key[i].ToString());
                s += int.Parse(key[i + 1].ToString());
                s += int.Parse(key[i + 2].ToString());
                decryptKey += (s % 10).ToString();

            }
            return decryptKey;

        }


        //Шифрование строки со сдвигом 4 в общем виде
        static public string Crypt(string str, int k)
        {
            var res = "";
            var alf = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            alf = alf + alf.ToLower();
            for (int i = 0; i < str.Length; i++)
            {
                var indx = alf.IndexOf(str[i]);
                if (indx == -1)
                {
                    res += str[i];
                }
                else
                {
                    var indx2 = (alf.Length + indx + k) % alf.Length;
                    res += alf[indx2];
                }
            }
            return res;
        }

        */


    }
}


/*
 ----------------------------------------
ИНФОРМАЦИЯ ПО КАРТЕ 1111111111111111
Номер карты: 1111111111111111
Имя владельца: Daria
Месяц и год окончания действия карты: 10.2025
Банк-эмитент карты: Sber
Сумма денег на счету: 15746

----------------------------------------
----------------------------------------
ИНФОРМАЦИЯ ПО КАРТЕ 1110311411115001
Номер карты: 1110311411115001
Имя владельца: Daria
Месяц и год окончания действия карты: 10.2029
Банк-эмитент карты: Tinkoff
Сумма денег на счету: 0

----------------------------------------
----------------------------------------
ИНФОРМАЦИЯ ПРО ATM
ID Банка: 2065338299
Банк, которому принадлежит банкомат: Sber
Секретный ключ для валидации инкассаторов: hahaha

----------------------------------------
Общее кол-во купюр в банкомате: 950000
----------------------------------------
----------------------------------------
ПОПОЛНЕНИЕ КАРТЫ 1111111111111111:
Сообщения от банкомата
System.Exception: Не найден данный номинал 231
----------------------------------------
ИНФОРМАЦИЯ ПО КАРТЕ 1111111111111111
Номер карты: 1111111111111111
Имя владельца: Daria
Месяц и год окончания действия карты: 10.2025
Банк-эмитент карты: Sber
Сумма денег на счету: 15946

----------------------------------------
ПОПОЛНЕНИЕ КАРТЫ 1110311411115001:
Сообщения от банкомата
Банк-эмитент карты не совпадает с банком, владеющим банкоматом. Вводится комиссия 5%
Банк-эмитент карты не совпадает с банком, владеющим банкоматом. Вводится комиссия 5%
Банк-эмитент карты не совпадает с банком, владеющим банкоматом. Вводится комиссия 5%
----------------------------------------
ИНФОРМАЦИЯ ПО КАРТЕ 1110311411115001
Номер карты: 1110311411115001
Имя владельца: Daria
Месяц и год окончания действия карты: 10.2029
Банк-эмитент карты: Tinkoff
Сумма денег на счету: 4275

----------------------------------------
----------------------------------------
СНЯТИЕ С КАРТЫ 1111111111111111:
Сообщения от банкомата
----------------------------------------
ИНФОРМАЦИЯ ПО КАРТЕ 1111111111111111
Номер карты: 1111111111111111
Имя владельца: Daria
Месяц и год окончания действия карты: 10.2025
Банк-эмитент карты: Sber
Сумма денег на счету: 15446

----------------------------------------
СНЯТИЕ С КАРТЫ 1110311411115001:
Сообщения от банкомата
Банк-эмитент карты не совпадает с банком, владеющим банкоматом. Вводится комиссия 5%
----------------------------------------
ИНФОРМАЦИЯ ПО КАРТЕ 1110311411115001
Номер карты: 1110311411115001
Имя владельца: Daria
Месяц и год окончания действия карты: 10.2029
Банк-эмитент карты: Tinkoff
Сумма денег на счету: 2850

----------------------------------------
КЛЮЧИ НЕ СОВПАДАЮТ! ПОЛИЦИЯ УЖЕ ЗА ДВЕРЬЮ!
----------------------------------------
КАССЕТА ИЗВЛЕЧЕНА
ИСТОРИЯ ТРАНЗАКЦИЙ БАНКОМАТА
1111111111111111: пополнение (100) => выполнено
1111111111111111: пополнение (100) => выполнено
1111111111111111: пополнение (462) => error
1110311411115001: пополнение (950 - комиссия 5%) => выполнено
1110311411115001: пополнение (950 - комиссия 5%) => выполнено
1110311411115001: пополнение (2375 - комиссия 5%) => выполнено
1111111111111111: снятие (500) => выполнено
1110311411115001: снятие (1425 - комиссия 5%) => выполнено
----------------------------------------
ИНФОРМАЦИЯ ПРО ATM
ID Банка: 2065338299
Банк, которому принадлежит банкомат: Sber
Секретный ключ для валидации инкассаторов: hahaha

----------------------------------------
Общее кол-во купюр в банкомате: 0
----------------------------------------

 */

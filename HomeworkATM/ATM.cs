using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkATM
{
    public class ATM
    {
        public long bankID { get; private set; }
        public string ownerATM { get; private set; }

        public Dictionary<string, Stack<Banknote>> ATMkass;

        public List<string> transHistory; // [<карта>: <вид транзакции> (<сумма>) => <ответ банкомата>]

        public string validKey;

        public ATM(string ownerATM, Dictionary<string, Stack<Banknote>> ATMDictionary, List<string> transHistory, string validKey)
        {
            var r = new Random();
            this.bankID = r.Next();
            this.ownerATM = ownerATM;
            this.ATMkass = ATMDictionary;
            this.transHistory = transHistory;
            this.validKey = validKey;
        }


        public string ToString()
        {
            return $"ID Банка: {bankID} \nБанк, которому принадлежит банкомат: {ownerATM} \nСекретный ключ для валидации инкассаторов: {validKey}\n";
        }


        //возвращает общее количество купюр в кассете банкомата.
        public int CashAmount()
        {
            var s = 0;
            foreach (var item in ATMkass.Values)
            {
                s += item.Count;
            }
            return s;
        }


        //показывает историю транзакций
        public void ShowTrans()
        {
            Console.WriteLine("ИСТОРИЯ ТРАНЗАКЦИЙ БАНКОМАТА");
            var l = transHistory.ToList();
            foreach (var item in l)
            {
                Console.WriteLine(item);
            }
        }



        public void ToppingUpCard(Card card, Dictionary<string, Banknote> setting)
        {
            foreach (var key in setting.Keys)
            {
                if (!CheckIssuingBank(card))
                {
                    Console.WriteLine("Банк-эмитент карты не совпадает с банком, владеющим банкоматом. Вводится комиссия 5%");
                    transHistory.Add($"{card.cardNumber}: пополнение ({int.Parse(key) - 0.05 * int.Parse(key)} - комиссия 5%) => выполнено");
                    ChangingBalance(card, int.Parse(key) - 0.05 * int.Parse(key));
                    continue;
                }
                else if (!CheckSeries(setting[key].Series))
                {
                    Console.WriteLine("Банкнота не прошла проверку на подлинность!");
                    transHistory.Add($"{card.cardNumber}: пополнение ({int.Parse(key)}) => error");
                    continue;
                }
                else if (!CheckendDate(card))
                {
                    transHistory.Add($"{card.cardNumber}: пополнение ({int.Parse(key)}) => error");
                    Console.WriteLine(new Exception("Истек срок действия карты"));
                    continue;
                }
                else if (!ATMkass.ContainsKey(key))
                {
                    transHistory.Add($"{card.cardNumber}: пополнение ({int.Parse(key)}) => error");
                    Console.WriteLine(new Exception($"Не найден данный номинал {key}"));
                    continue;
                }
                else
                {
                    transHistory.Add($"{card.cardNumber}: пополнение ({int.Parse(key)}) => выполнено");
                    ChangingBalance(card, int.Parse(key));
                }
            }
        }

        // проверка серии банкноты
        public bool CheckSeries(string series)
        {
            var flag = false;
            if ((Math.Abs(series[0] - series[1]) - 1) % 2 == 0)
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

        public void WithdrawalCard(Card card, double sumRaw)
        {
            if (!CheckIssuingBank(card) && sumRaw <= card.moneySum)
            {
                Console.WriteLine("Банк-эмитент карты не совпадает с банком, владеющим банкоматом. Вводится комиссия 5%");
                transHistory.Add($"{card.cardNumber}: снятие ({sumRaw - 0.05 * sumRaw} - комиссия 5%) => выполнено");
                ChangingBalance(card, -(sumRaw - 0.05 * sumRaw));
                return;
            }
            else if (!CheckendDate(card))
            {

                transHistory.Add($"{card.cardNumber}: снятие ({sumRaw}) => error");
                Console.WriteLine(new Exception("Истек срок действия карты"));
                return;
            }
            else if (sumRaw > card.moneySum)
            {
                transHistory.Add($"{card.cardNumber}: снятие ({sumRaw}) => error");
                Console.WriteLine(new Exception($"На счету недостаточно средств для снятия суммы {sumRaw}"));
                return;
            }
            else
            {
                transHistory.Add($"{card.cardNumber}: снятие ({sumRaw}) => выполнено");
                ChangingBalance(card, -sumRaw);
                return;
            }
        }


        // изменение баланса карты
        public void ChangingBalance(Card card, double addSum)
        {
            card.moneySum += addSum;
        }

        //проверка даты истечения действия карты
        public bool CheckendDate(Card card)
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
        public bool CheckIssuingBank(Card card)
        {
            return card.issuingBank == ownerATM;
        }


        //ИНКАССАЦИЯ
        public void Pickup(string keyInc)
        {
            if (NewCrypt(keyInc) == validKey)
            {
                var dicHashSet = new Dictionary<string, HashSet<string>>(); // 1 номинал = 1 множество
                foreach (var item in ATMkass)
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
                foreach (var item in ATMkass)
                {
                    ATMkass[item.Key].Clear();
                }
                Console.WriteLine("КАССЕТА ИЗВЛЕЧЕНА");
                ShowTrans();
            }
            else
            {
                Console.WriteLine("КЛЮЧИ НЕ СОВПАДАЮТ! ПОЛИЦИЯ УЖЕ ЗА ДВЕРЬЮ!");
            }

        }

        // новый метод дешифрования
        public string NewCrypt(string key)
        {
            var decryptKey = "";
            for (int i = 0; i < key.Length - 2; i++)
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
        public string Crypt(string str, int k)
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



    }
}

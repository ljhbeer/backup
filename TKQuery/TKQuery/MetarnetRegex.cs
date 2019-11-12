
/***********************************************************
 * �ļ�: MetarnetRegex.cs
 * ����: 2006-07-25
 **********************************************************/
using System;
using System.Text.RegularExpressions;
namespace MetarCommonSupport
{
 /// <summary>
 /// ͨ��Framwork����е�Regex��ʵ����һЩ���⹦�����ݼ��
 /// </summary>
 public class MetarnetRegex
 {
  
  private static MetarnetRegex instance = null;
  public static MetarnetRegex GetInstance()
  {
   if(MetarnetRegex.instance == null)
   {
    MetarnetRegex.instance = new MetarnetRegex();
   }
   return MetarnetRegex.instance;
  }
  private MetarnetRegex()
  {
  }
  /// <summary>
  /// �ж�������ַ���ֻ��������
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static bool IsChineseCh(string input)
  {
   Regex regex = new Regex("^[\u4e00-\u9fa5]+$");
   return regex.IsMatch(input);
  }

  /// <summary>
  /// ƥ��3λ��4λ���ŵĵ绰���룬�������ſ�����С������������
  /// Ҳ���Բ��ã������뱾�غż���������ֺŻ�ո�����
  /// Ҳ����û�м��
  /// \(0\d{2}\)[- ]?\d{8}|0\d{2}[- ]?\d{8}|\(0\d{3}\)[- ]?\d{7}|0\d{3}[- ]?\d{7}
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static bool IsPhone(string input)
  {
   string pattern = "^\\(0\\d{2}\\)[- ]?\\d{8}$|^0\\d{2}[- ]?\\d{8}$|^\\(0\\d{3}\\)[- ]?\\d{7}$|^0\\d{3}[- ]?\\d{7}$";
   Regex regex = new Regex(pattern);
   return regex.IsMatch(input);
  }

  /// <summary>
  /// �ж�������ַ����Ƿ���һ���Ϸ����ֻ���
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static bool IsMobilePhone(string input)
  {
   Regex regex = new Regex("^13\\d{9}$");
   return regex.IsMatch(input);
   
  }


  /// <summary>
  /// �ж�������ַ���ֻ��������
  /// ����ƥ�������͸�����
  /// ^-?\d+$|^(-?\d+)(\.\d+)?$
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static bool IsNumber(string input)
  {
   string pattern = "^-?\\d+$|^(-?\\d+)(\\.\\d+)?$";
   Regex regex = new Regex(pattern);
   return regex.IsMatch(input);
  }
  /// <summary>
  /// ƥ��Ǹ�����
  ///
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static bool IsNotNagtive(string input)
  {
   Regex regex = new Regex(@"^\d+$");
   return regex.IsMatch(input);
  }
  /// <summary>
  /// ƥ��������
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static bool IsUint(string input)
  {
   Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");
   return regex.IsMatch(input);
  }
  /// <summary>
  /// �ж�������ַ����ְ���Ӣ����ĸ
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static bool IsEnglisCh(string input)
  {
   Regex regex = new Regex("^[A-Za-z]+$");
   return regex.IsMatch(input);
  }


  /// <summary>
  /// �ж�������ַ����Ƿ���һ���Ϸ���Email��ַ
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static bool IsEmail(string input)
  {
   string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
   Regex regex = new Regex(pattern);
   return regex.IsMatch(input);
  }


  /// <summary>
  /// �ж�������ַ����Ƿ�ֻ�������ֺ�Ӣ����ĸ
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static bool IsNumAndEnCh(string input)
  {
   string pattern = @"^[A-Za-z0-9]+$";
   Regex regex = new Regex(pattern);
   return regex.IsMatch(input);
  }


  /// <summary>
  /// �ж�������ַ����Ƿ���һ��������
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static bool IsURL(string input)
  {
   //string pattern = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
   string pattern = @"^[a-zA-Z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$";
   Regex regex = new Regex(pattern);
   return regex.IsMatch(input);
  }


  /// <summary>
  /// �ж�������ַ����Ƿ��Ǳ�ʾһ��IP��ַ
  /// </summary>
  /// <param name="input">���Ƚϵ��ַ���</param>
  /// <returns>��IP��ַ��ΪTrue</returns>
  public static bool IsIPv4(string input)
  {
   
   string[] IPs = input.Split('.');
   Regex regex = new Regex(@"^\d+$");
   for(int i = 0; i<IPs.Length; i++)
   {
    if(!regex.IsMatch(IPs[i]))
    {
     return false;
    }
    if(Convert.ToUInt16(IPs[i]) > 255)
    {
     return false;
    }
   }
   return true;
  }


  /// <summary>
  /// �����ַ������ַ����ȣ�һ�������ַ���������Ϊ�����ַ�
  /// </summary>
  /// <param name="input">��Ҫ������ַ���</param>
  /// <returns>�����ַ����ĳ���</returns>
  public static int GetCount(string input)
  {
   return Regex.Replace(input,@"[\u4e00-\u9fa5/g]","aa").Length;
  }

  /// <summary>
  /// ����Regex��IsMatch����ʵ��һ���������ʽƥ��
  /// </summary>
  /// <param name="pattern">Ҫƥ���������ʽģʽ��</param>
  /// <param name="input">Ҫ����ƥ������ַ���</param>
  /// <returns>���������ʽ�ҵ�ƥ�����Ϊ true������Ϊ false��</returns>
  public static bool IsMatch(string pattern, string input)
  {
   Regex regex = new Regex(pattern);
   return regex.IsMatch(input);
  }
  
  /// <summary>
  /// �������ַ����еĵ�һ���ַ���ʼ�����滻�ַ����滻ָ����������ʽģʽ������ƥ���
  /// </summary>
  /// <param name="pattern">ģʽ�ַ���</param>
  /// <param name="input">�����ַ���</param>
  /// <param name="replacement">�����滻���ַ���</param>
  /// <returns>���ر��滻��Ľ��</returns>
  public static string Replace(string pattern, string input, string replacement)
  {
   Regex regex = new Regex(pattern);
   return regex.Replace(input,replacement);
  }

  /// <summary>
  /// ����������ʽģʽ�����λ�ò�������ַ�����
  /// </summary>
  /// <param name="pattern">ģʽ�ַ���</param>
  /// <param name="input">�����ַ���</param>
  /// <returns></returns>
  public static string[] Split(string pattern, string input)
  {
   Regex regex = new Regex(pattern);
   return regex.Split(input);
  }
  /// <summary>
  /// �ж�������ַ����Ƿ��ǺϷ���IPV6 ��ַ
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static bool IsIPV6(string input)
  {
   string pattern = "";
   string temp = input;
   string[] strs = temp.Split(':');
   if(strs.Length > 8)
   {
    return false;
   }
   int count = MetarnetRegex.GetStringCount(input,"::");
   if(count>1)
   {
    return false;
   }
   else if(count == 0)
   {
    pattern = @"^([\da-f]{1,4}:){7}[\da-f]{1,4}$";

    Regex regex = new Regex(pattern);
    return regex.IsMatch(input);
   }
   else
   {
    pattern = @"^([\da-f]{1,4}:){0,5}::([\da-f]{1,4}:){0,5}[\da-f]{1,4}$";
    Regex regex1 = new Regex(pattern);
    return regex1.IsMatch(input);
   }

  }
  /* *******************************************************************
   * 1��ͨ����:�����ָ��ַ������õ����ַ������鳤���Ƿ�С�ڵ���8
   * 2���ж������IPV6�ַ������Ƿ��С�::����
   * 3�����û�С�::������ ^([\da-f]{1,4}:){7}[\da-f]{1,4}$ ���ж�
   * 4������С�::�� ���ж�"::"�Ƿ�ֹ����һ��
   * 5���������һ������ ����false
   * 6��^([\da-f]{1,4}:){0,5}::([\da-f]{1,4}:){0,5}[\da-f]{1,4}$
   * ******************************************************************/
  /// <summary>
  /// �ж��ַ���compare �� input�ַ����г��ֵĴ���
  /// </summary>
  /// <param name="input">Դ�ַ���</param>
  /// <param name="compare">���ڱȽϵ��ַ���</param>
  /// <returns>�ַ���compare �� input�ַ����г��ֵĴ���</returns>
  private static int GetStringCount(string input, string compare)
  {
   int index = input.IndexOf(compare);
   if(index != -1)
   {
    return 1 + GetStringCount(input.Substring(index + compare.Length),compare);
   }
   else
   {
    return 0;
   }

  }
 }
}


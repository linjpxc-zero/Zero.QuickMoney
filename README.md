# Zero.QuickMoney

# 说明

Zero.QuockMoney 提供一个表示货币与金额的类库，旨在提供一个统一的货币接口。该库有三大核心接口：

- ICurrency : ` 描述一个具体的货币信息 `
- IMoney : ` 描述一个确定货币 ICurrency 的金额 `
- IExchangeRate : ` 提供一个将不同货币相互转换的接口 `

三大核心接口都有对应的默认实现 CurrencyInfo，Money 和 ExchangeRate。

# 三大对象基本用法

## CyrrencyInfo

```C#
// 获取 ISO-4217 货币信息
var currency = CurrencyInfo.FromCode("CNY");
var blRet = CurrencyInfo.TryFromCode("CNY", out var currency);

// 获取当前的货币信息
var currency = CurrencyInfo.CurrentCurrency;

// 通过 ISO-4217 的数字编码获取货币信息
var currency = CurrencyInfo.FromNumeric("156");
var blRet = CurrencyInfo.TryFromNumeric("156", out var currency);

// 通过指定区域信息，获取货币信息
var currency = CurrencyInfo.FromRegion(new RegionInfo("zh-CN"));
var blRet = CurrencyInfo.TryFromRegion(new RegionInfo("zh-CN", out var currency));

// 通过指定的文化信息，获取货币信息
var currency = CurrencyInfo.FromCulture(CultureInfo.CurrencyCulture);
var blRet = CurrencyInfo.TryFromCulture(CultureInfo.CurrencyCulture, out var currency);
```

## Money

```C#
// 使用当前货币信息 CurrencyInfo.CurrentCurrency 创建金额
var money = (Money)100M;
var money = Money.Parse("100");

// 创建指定货币信息的金额
var money = Money.Parse("CNY100");
var money = Money.Parse("CNY 100");
var money = Money.Parse("CNY ¥100");

// Money 可以当作一个 decimal 类型来使用，支持加减乘除等基本运算
var money1 = (Money)1;
var money2 = (Money)2;
var money = money1 + money2; // money: 3
```

## ExchangeRate

```C#
// 创建汇率转换对象
var exchangeRate = ExchangeRate.Parse("CNYUSD 7");
var exchangeRate = ExchangeRate.Parse("CNY/USD 7");
```

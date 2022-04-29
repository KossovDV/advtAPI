
# ADs Test API

API have three methods:

| Name             | Description                                                                |
| ----------------- | ------------|
| `GetAds` | Return list of ads |
| `GetAdById` | Return specifed ad |
| `PutAd` |  Inserting ad to database| 


## GetAds

Method: `GET`

**GetAds** accepting three field:

`PageNumber` - **zero based** counter that display current requested page (if PageNumber is more than total amount of pages request return empty list of ads).

`DateSort` - field that accepting three values for sorting ad records by creation date. Values `ASC` `DESC` or `0` for skip sorting by date.

`PriceSort`  field that accepting three values for sorting ad records by ad price. Values `ASC` `DESC` or `0` for skip sorting by price.

**In Types**
| Name             | Type |
| ----------------- | ------------|
| `PageNumber` | `int` |
| `DateSort` | `string` |
| `PriceSort` |  `string`| 



**Return fields:**


`page` - show current page.

`per_page` - show max records in one page.

`total_pages` - show amount of pages.

`AdsList` - array that contain ads.

Ad array fields:

`AdName` - name of ad.

`AdPrice` - ad price.

`AdMainLink` - show main ad photo link.

**Return Types**
| Name             | Type |
| ----------------- | ------------|
| `page` | `int` |
| `per_page` | `int` |
| `total_pages` |  `int`| 
| `AdsList` |  `array`|
| `AdName` | `string` |
| `AdPrice` | `double` |
| `AdMainLink` |  `string`| 


Request example:
```
/api/Ads/GetAds?PageNumber=0&DateSort=ASC&PriceSort=DESC
```

Return example:

```
{
    "page": 0,
    "per_page": 10,
    "total_pages": 3,
    "AdsList": [
        {
            "AdName": "First Add Ad",
            "AdMainLink": "https://images.pexels.com/photos/10873117/pexels-photo-10873117.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
            "AdPrice": 17.01
        },
        {
            "AdName": "Ad with stock images",
            "AdMainLink": "https://images.pexels.com/photos/10913514/pexels-photo-10913514.jpeg?auto=compress&cs=tinysrgb&dpr=3&h=750&w=1260",
            "AdPrice": 15.64
        },
        *
        *        
        *
        {
            "AdName": "Dummy ads",
            "AdMainLink": "link1",
            "AdPrice": 1.74
        }
    ]
}
```
## GetAdById

Method: `GET`

**GetAdById** accepting two field:

`AdId` - number that representing ad in database.

`fields` - option that allow user get additional fields `AdDescription` and all ad photolinks in field `AdLinks`.

**In Types**
| Name             | Type |
| ----------------- | ------------|
| `AdId` | `int` |
| `fields` | `bolean` |

**Return fields:**

`AdName` - number that representing ad in database.

`AdPrice` - ad price.

`AdLinks` - if parameter `fields` set to `false` return firs Ad photo link if parameter set to `true` return all ad photo links sepatated by **','**.

`AdDescription` - if parameter `fields` set to `true` return ad description field.

**Return Types**
| Name             | Type |
| ----------------- | ------------|
| `AdName` | `string` |
| `AdPrice` | `double` |
| `AdLinks` |  `string`| 
| `AdDescription` |  `string`| 


Request example with `fields` is `true`:
```
/api/Ads/GetAdById?AdId=6&fields=true
```

Will return:

```
{
    "AdName": "Dummy add desc text",
    "AdPrice": 584,
    "AdDescription": "Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in Bookmarksgrove right at the coast of the Semantics, a large language ocean. A small river named Duden flows by their place and supplies it with the necessary regelialia. It is a paradisematic country, in which roasted parts of sentences fly into your mouth. Even the all-powerful Pointing has no control about the blind texts it ",
    "AdLinks": "link1,link2,link3"
}
```

Request example with `fields` is `true`:
```
/api/Ads/GetAdById?AdId=6&fields=false
```

Will return:
```
{
    "AdName": "Dummy add desc text",
    "AdPrice": 584,
    "AdLinks": "link1"
}
```
## PutAd

Method: `POST`

**PutAd** accepting four field:

`AdName` - new ad name **(max lenth 200 chars)**.

`AdPrice` - new ad price.

`AdDescription` - new ad description  **(max lenth 1000 chars)**.

`AdLinks` - new ad photo links **(max lenth 3 link sepatated by ',')**.


**In Types**
| Name             | Type |
| ----------------- | ------------|
| `AdName` | `string` |
| `AdPrice` | `double` |
| `AdDescription` | `string` |
| `AdLinks` | `string` |

**Return fields:**

`code` - error code return `0` if secsessful.

`AdId` - ad id in database.

**Return Types**
| Name             | Type |
| ----------------- | ------------|
| `code` | `string` |
| `AdId` | `string` |



Request example:
```
/api/Ads/PutAd?AdName=The quick, brown fox jumps over a lazy dog.&AdPrice=7.21&AdDescription=Testo&AdLinks=link1
```

Will return:

```
[
    "Code:0",
    "AdId:23"
]
```

Request example with error:
```
/api/Ads/PutAd?AdName=The quick, brown fox jumps over a lazy dog. DJs flock by when MTV ax quiz prog. Junk MTV quiz graced by fox whelps. Bawds jog, flick quartz, vex nymphs. Waltz, bad nymph, for quick jigs vex! Fox nymphs grab&AdPrice=7.35&AdDescription=Testo&AdLinks=link1
```

Will return:
```
[
    "Code:1",
    "Error"
]
```

**Error cores:**

`1` - "Error! AD Name length more that 200 symbols"

`2` - "Error! AD Description length more that 1000 symbols"

`3` - "Error! AD PhotoLinks cannot be more that 3 links"

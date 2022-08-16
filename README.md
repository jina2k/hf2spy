# hf2spy
comparing large holdings from hedge funds to the s&amp;p 500 using 13fs

## How to Use
-Go to xmlproject/bin/Debug/netcoreapp3.1

-Launch xmlproject

## Resources

~ https://www.ssga.com/us/en/individual/etfs/funds/spdr-sp-500-etf-trust-spy - For finding the CUSIP/ticker symbol of every ticker in the s&p 500

~ 13fs of hedge funds such as citadel, credit suisse, used to cross-match CUSIP, add total market value. (XML Format)

Citadel:
</br>
https://www.sec.gov/cgi-bin/browse-edgar?CIK=0001423053
</br>
Credit Suisse:
</br>
https://www.sec.gov/edgar/browse/?CIK=824468
</br>
Blackrock:
</br>
https://www.sec.gov/edgar/browse/?CIK=1364742
</br>

~ stackoverflow

~ https://stockmarketmba.com/index.php - used search bar to search up cusip because the s&p 500 data had incorrect CUSIP numbers.

~ http://www.jenitennison.com/xslt/grouping/muenchian.xml - to understand the Muenchian method used to group XML attributes together in an attempt to reduce operating time


The purpose of this project is to see how much exposure a hedge fund has for each stock. Essentially a modification of whalewisdom by referencing s&p 500 holdings only.
The data is inaccurate in a way that hedge funds can simply lie and get fined a small amount on their 13fs.
Calls/Puts are also listed for an idea of "market value" exposure that hedge funds have for each ticker.

This project was done for fun and out of curiosity (informational purposes), seeing how much a hedge fund would "own" in each ticker in the s&p 500.

## Methods

~ csv to xml for s and p 500 data (using Linq to create XML files)

~ combining two xml files into one (using LINQ)

~ fixing CUSIP data (creating a web query tool and storing results, code not included in repo)

~ Muenchian method to group XML attributes together (used XSLT 1.0)

### Operation Time (using Blackrock file, in order of progress within the project):

No Muenchian Method, used XPath2: 7 minutes

Muenchian Method, used XPath2: 1 minute

XSLT 2.0 with Saxon API, used XPath2: 4 minutes

Muenchian Method, LINQ: 10 seconds

## Changes

Added an option for checking holdings in general, sorted by value. This option does not include the ticker, and instead uses the nameofIssuer from standard 13f filings.

~ https://github.com/StefH/XPath2.Net - Removed XPath2, primarily used it for combining 2 XML files, which was then replaced with just using LINQ.

Also fixed edge cases where the entire string of the CUSIP did not transfer properly when scraping to csv, so now the sandp500holdings.xml file contains CUSIP lengths of only 9. (standardized to match XML files from SEC database)

Transitioned into using JSON to display end result instead of XML. (Intended to bypass CORS doing this, realized I just needed to do XSL transformation beforehand. Also, JSON also has CORS issues unless I declare the data in a javascript file under a variable to be used in the output file.)

Archived files from previous steps in the project into the branch "main". Branch "stable" will be used as the main branch now.

## Future Updates

With JSON being used now, the next step for the project is displaying the end result in a more modern like fashion with the use of CSS and related libraries.

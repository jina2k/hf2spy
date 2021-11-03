# hf2spy
comparing large holdings from hedge funds to the s&amp;p 500 using 13fs

## How to Use
-Go to xmlproject

-Launch xmlproject(shortcut)

## Resources

~ https://www.ssga.com/us/en/individual/etfs/funds/spdr-sp-500-etf-trust-spy - For finding the CUSIP/ticker symbol of every ticker in the s&p 500

~ 13fs of hedge funds such as citadel, credit suisse, used to cross-match CUSIP, add total market value. (XML Format)

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

## Future Updates

In the future, the project could transition into using JSON for displaying the end result instead. This would be a lot easier to work with and would be able to bypass some same-origin issues (https://stackoverflow.com/questions/3420513/firefox-and-remote-xsl-stylesheets), allowing the end result to be displayed on multiple browsers.

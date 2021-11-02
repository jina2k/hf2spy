# hf2spy
comparing large holdings from hedge funds to the s&amp;p 500 using 13fs

## Resources

~ https://www.ssga.com/us/en/individual/etfs/funds/spdr-sp-500-etf-trust-spy - For finding the CUSIP/ticker symbol of every ticker in the s&p 500

~ 13fs of hedge funds such as citadel, credit suisse, used to cross-match CUSIP, add total market value. (XML Format)

~ https://github.com/StefH/XPath2.Net - For documentation on how to use XPath 2.0, crucial for trying to interact between two xml files (s&p 500 holdings csv to xml, compared with the 13fs of hedge funds)

~ stackoverflow

~ https://stockmarketmba.com/index.php - used search bar to search up cusip because the s&p 500 data had incorrect CUSIP numbers.

~ http://www.jenitennison.com/xslt/grouping/muenchian.xml - to understand the Muenchian method used to group XML attributes together in an attempt to reduce operating time


The purpose of this project is to see how much exposure a hedge fund has for each stock. Essentially a modification of whalewisdom by referencing s&p 500 holdings only.
The data is inaccurate in a way that hedge funds can simply lie and get fined a small amount on their 13fs.
Calls/Puts are also listed for an idea of "market value" exposure that hedge funds have for each ticker.

This project was done for fun and out of curiosity (informational purposes), seeing how much a hedge fund would "own" in each ticker in the s&p 500.

## Methods

~ csv to xml for s and p 500 data (using Linq to create XML files)

~ combining two xml files into one (using XPath 2.0)

~ fixing CUSIP data (creating a web query tool and storing results, code not included in repo)

~ Muenchian method to group XML attributes together (used XSLT 1.0)

Currently the estimated time for running the application on a file (on battery) is 1 minute (using Blackrock's 13F holdings for May of 2021). Citadel's 13F in comparison takes around 50 seconds. (before grouping and removing extraneous variables, Blackrock took 7 minutes and Citadel took 2 minutes) When plugged in, running the application takes approximately 10 seconds less time.


## Changes

Added an option for checking holdings in general, sorted by value. This option does not include the ticker, and instead uses the nameofIssuer from standard 13f filings.

### Thoughts on XSLT 2.0 and further updates
Attempted an update to XSLT 2.0, upon looking over the project again. Referenced https://en.wikipedia.org/wiki/XSLT/Muenchian_grouping as well as https://www.xml.com/pub/a/2003/11/05/tr.html
Currently, the performance with XSLT 2.0 is actually worse than XSLT 1.0 (using Muenchian). The likely cause is probably due to the usage of Saxon's API. I would say it's around 2-3 times slower than XSLT 1.0, especially when testing with a big file like blackrock. This is very apparent when the code takes much longer during the initial stage, before the console prompts the user whether or not to cross-check the results against the s&p 500. As a result, I did not continue parsing the data for the end file. 

Due to how 13F filings generally work (companies submit them quarterly), I don't see further point in attempting to improve parsing the data.
However, in the future, the project could transition into using JSON for displaying the end result instead. This would be a lot easier to work with and would be able to bypass some same-origin issues (https://stackoverflow.com/questions/3420513/firefox-and-remote-xsl-stylesheets), allowing the end result to be displayed on multiple browsers.

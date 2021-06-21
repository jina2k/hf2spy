# hf2spy
comparing large holdings from hedge funds to the s&amp;p 500 using 13fs

Resources used:

~ https://www.ssga.com/us/en/individual/etfs/funds/spdr-sp-500-etf-trust-spy - For finding the CUSIP/ticker symbol of every ticker in the s&p 500

~ 13fs of hedge funds such as citadel, credit suisse, used to cross-match CUSIP, add total market value. (XML Format)

~ https://github.com/StefH/XPath2.Net - For documentation on how to use XPath 2.0, crucial for trying to interact between two xml files (s&p 500 holdings csv to xml, compared with the 13fs of hedge funds)

~ stackoverflow

The purpose of this project is to see how much exposure a hedge fund has for each stock. Essentially a modification of whalewisdom by referencing s&p 500 holdings only.
The data is inaccurate in a way that hedge funds can simply lie and get fined a small amount on their 13fs.
Calls/Puts are also listed for an idea of "market value" exposure that hedge funds have for each ticker.

This project was done for fun and out of curiosity (informational purposes), seeing how much a hedge fund would "own" in each ticker in the s&p 500.

# hf2spy
comparing large holdings from hedge funds to the s&amp;p 500 using 13fs

Resources used:
https://www.ssga.com/us/en/individual/etfs/funds/spdr-sp-500-etf-trust-spy - For finding the CUSIP/ticker symbol of every ticker in the s&p 500
13fs of hedge funds such as citadel, credit suisse, used to cross-match CUSIP, add total market value.

The purpose of this project is to see how much exposure a hedge fund has for each stock. Essentially replicating whalewisdom but referencing s&p 500 only.

The data is inaccurate in a way that hedge funds can simply lie and get fined a small amount on their 13fs.

Calls/Puts are also not actually shares, but are treated the same for an idea of "market value" exposure that hedge funds have for each ticker.

This project was done for fun and out of curiosity, seeing how much a hedge fund would "own" in each ticker in the s&p 500,
which could be used for various things, especially when speculating a market crash.


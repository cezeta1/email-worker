var juice = require('juice');
var htmlMinify = require('html-minifier').minify; 
var Inky = require('inky').Inky;
var cheerio = require('cheerio');

const inkify = (input) => new Promise(
    res => res(
        new Inky({}).releaseTheKraken(cheerio.load(input))
    )
);

const juiceify = (input, _relativeTo) => new Promise(
    (resolve, reject) => 
        juice.juiceResources(
            input,
            { webResources: { relativeTo: _relativeTo, images: false } },
            (err, html) => err ? reject(err) : resolve(html))
);

const minify = (input) => new Promise(
    res => res(
        htmlMinify(input, { removeComments: true, collapseWhitespace: true, removeStyleLinkTypeAttributes:true })
    )
);

// === Export for Jering.Javascript.NodeJS === //

module.exports = async (input, relativeTo) =>
    await inkify(input)
        .then(step1 => juiceify(step1, relativeTo))
        .then(step2 => minify(step2));
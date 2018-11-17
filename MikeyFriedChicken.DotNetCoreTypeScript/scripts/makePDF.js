"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var pdfMake = require("pdfmake/build/pdfmake");
var pdfFonts = require("pdfmake/build/vfs_fonts");
// tslint:disable-next-line:typedef
module.exports = function (callback, filename) {
    var docDefinition = {
        content: [
            // if you don't need styles, you can use a simple string to define a paragraph
            "This is a standard paragraph, using default style",
            // using a { text: '...' } object lets you set styling properties
            { text: "This paragraph will have a bigger font", fontSize: 15 },
            // if you set pass an array instead of a string, you'll be able
            // to style any fragment individually
            {
                text: [
                    "This paragraph is defined as an array of elements to make it possible to ",
                    { text: "restyle part of it and make it bigger ", fontSize: 15 },
                    "than the rest."
                ]
            }
        ]
    };
    pdfMake.vfs = pdfFonts.pdfMake.vfs;
    var pdf = pdfMake.createPdf(docDefinition);
    pdf.download("" + filename);
    //fs.readFile(`${filename}`, "utf8", (error: any, data: any) => {
    //    callback(null, data);
    //});
    callback(null, pdf);
};
//# sourceMappingURL=makePDF.js.map
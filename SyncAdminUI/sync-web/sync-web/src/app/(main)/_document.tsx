
import Document, { Html, Head, Main, NextScript } from 'next/document';

class MyDocument extends Document {
  render() {
    return (
      <Html>
        <Head>
          <meta charSet="UTF-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1.0" />
          <meta httpEquiv="X-UA-Compatible" content="ie=edge" />
          <title>Rainblur Landing Page Template: Tailwind Toolbox</title>
          <meta name="description" content="" />
          <meta name="keywords" content="" />
          <link rel="stylesheet" href="https://unpkg.com/tailwindcss@2.2.19/dist/tailwind.min.css" />
          {/* Replace with your tailwind.css once created */}
          <link href="https://unpkg.com/@tailwindcss/custom-forms/dist/custom-forms.min.css" rel="stylesheet" />
          <style>
            {`@import url("https://fonts.googleapis.com/css2?family=Poppins:wght@400;700&display=swap");

              html {
                font-family: "Poppins", -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, "Noto Sans", sans-serif, "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol", "Noto Color Emoji";
              }
            `}
          </style>
        </Head>
        <body className="leading-normal tracking-normal text-indigo-400 m-6 bg-cover bg-fixed" style={{ backgroundImage: "url('header.png')" }}>
          <Main />
          <NextScript />
        </body>
      </Html>
    );
  }
}

export default MyDocument;

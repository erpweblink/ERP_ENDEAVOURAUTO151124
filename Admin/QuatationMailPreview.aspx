<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QuatationMailPreview.aspx.cs" Inherits="Admin_QuatationMailPreview" %>

<html lang="en">
<head>
    <style>
        body {
            margin: 0;
            padding: 0;
            overflow-x: auto !important;
            overflow-y: hidden !important
        }

        .mail-detail-content {
            box-sizing: border-box;
            font-family: -apple-system,BlinkMacSystemFont,"Helvetica Neue","Segoe UI",Arial,sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol";
            font-size: 13px;
            font-weight: 400;
            font-feature-settings: "liga" 0;
            width: 100%;
            position: relative;
            padding: 0
        }

        .ios.smartphone .mail-detail-content {
            -webkit-overflow-scrolling: touch;
            overflow-x: auto
        }

        .smartphone .mail-detail-content {
            font-size: 15px
        }

        .mail-detail-content > div > [class$="-content"] {
            padding: 0
        }

        .mail-detail-content.plain-text {
            font-family: -apple-system,BlinkMacSystemFont,"Helvetica Neue","Segoe UI",Arial,sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol";
            white-space: pre-wrap
        }

            .mail-detail-content.plain-text blockquote {
                white-space: normal
            }

        .mail-detail-content.fixed-width-font, .mail-detail-content.fixed-width-font.plain-text, .mail-detail-content.fixed-width-font blockquote, .mail-detail-content.fixed-width-font.plain-text blockquote, .mail-detail-content.fixed-width-font blockquote p, .mail-detail-content.fixed-width-font.plain-text blockquote p {
            font-family: monospace;
            -webkit-font-feature-settings: normal;
            font-feature-settings: normal
        }

        .mail-detail-content.simple-mail {
            max-width: 700px
        }

            .mail-detail-content.simple-mail.big-screen {
                max-width: 100%
            }

            .mail-detail-content.simple-mail img {
                max-width: 100%;
                height: auto !important
            }

        .mail-detail-content img[src=""] {
            background-color: rgba(0,0,0,.1);
            background-image: repeating-linear-gradient(45deg,transparent,transparent 20px,rgba(255,255,255,.5) 20px,rgba(255,255,255,.5) 40px)
        }

        .mail-detail-content p {
            font-family: -apple-system,BlinkMacSystemFont,"Helvetica Neue","Segoe UI",Arial,sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol";
            margin: 0 0 1em
        }

        .mail-detail-content h1 {
            font-size: 28px
        }

        .mail-detail-content h2 {
            font-size: 21px
        }

        .mail-detail-content h3 {
            font-size: 16.38px
        }

        .mail-detail-content h4 {
            font-size: 14px
        }

        .mail-detail-content h5 {
            font-size: 11.62px
        }

        .mail-detail-content h6 {
            font-size: 9.38px
        }

        .mail-detail-content a {
            word-break: break-word;
            text-decoration: none;
            color: inherit
        }

            .mail-detail-content a:hover {
                color: inherit
            }

            .mail-detail-content a[href] {
                color: #3c61aa;
                text-decoration: underline
            }

        .mail-detail-content th {
            padding: 8px;
            text-align: center
        }

            .mail-detail-content th[align=left] {
                text-align: left
            }

        .mail-detail-content .calendar-detail .label {
            display: block;
            text-shadow: none;
            font-weight: 400;
            background-color: transparent
        }

        .mail-detail-content img.emoji-softbank {
            margin: 0 2px
        }

        .mail-detail-content pre {
            word-break: keep-all;
            word-break: initial;
            white-space: pre-wrap;
            background-color: transparent;
            border: 0 none;
            border-radius: 0
        }

        .mail-detail-content table {
            font-family: -apple-system,BlinkMacSystemFont,"Helvetica Neue","Segoe UI",Arial,sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol";
            font-size: 13px;
            font-weight: 400;
            font-feature-settings: "liga" 0;
            line-height: normal;
            border-collapse: collapse
        }

        .mail-detail-content ul, .mail-detail-content ol {
            padding: 0;
            padding-left: 16px;
            margin: 1em 0 1em 24px
        }

        .mail-detail-content ul {
            list-style-type: disc
        }

            .mail-detail-content ul ul {
                list-style-type: circle
            }

                .mail-detail-content ul ul ul {
                    list-style-type: square
                }

        .mail-detail-content li {
            line-height: normal;
            margin-bottom: .5em
        }

        .mail-detail-content blockquote {
            color: #555;
            font-size: 13px;
            border-left: 2px solid #ddd;
            padding: 0 0 0 16px;
            margin: 16px 0
        }

            .mail-detail-content blockquote p {
                font-size: 13px
            }

            .mail-detail-content blockquote blockquote {
                border-color: #283f73;
                margin: 8px 0
            }

        .mail-detail-content.colorQuoted blockquote blockquote {
            color: #283f73 !important;
            border-left: 2px solid #283f73
        }

            .mail-detail-content.colorQuoted blockquote blockquote a[href]:not(.deep-link) {
                color: #283f73
            }

                .mail-detail-content.colorQuoted blockquote blockquote a[href]:not(.deep-link):hover {
                    color: #1b2a4d
                }

            .mail-detail-content.colorQuoted blockquote blockquote blockquote {
                color: #dd0880 !important;
                border-left: 2px solid #dd0880
            }

                .mail-detail-content.colorQuoted blockquote blockquote blockquote a[href]:not(.deep-link) {
                    color: #dd0880
                }

                    .mail-detail-content.colorQuoted blockquote blockquote blockquote a[href]:not(.deep-link):hover {
                        color: #ac0663
                    }

                .mail-detail-content.colorQuoted blockquote blockquote blockquote blockquote {
                    color: #8f09c7 !important;
                    border-left: 2px solid #8f09c7
                }

                    .mail-detail-content.colorQuoted blockquote blockquote blockquote blockquote a[href]:not(.deep-link) {
                        color: #8f09c7
                    }

                        .mail-detail-content.colorQuoted blockquote blockquote blockquote blockquote a[href]:not(.deep-link):hover {
                            color: #6c0796
                        }

                    .mail-detail-content.colorQuoted blockquote blockquote blockquote blockquote blockquote {
                        color: #767676 !important;
                        border-left: 2px solid #767676
                    }

                        .mail-detail-content.colorQuoted blockquote blockquote blockquote blockquote blockquote a[href]:not(.deep-link) {
                            color: #767676
                        }

                            .mail-detail-content.colorQuoted blockquote blockquote blockquote blockquote blockquote a[href]:not(.deep-link):hover {
                                color: #5d5d5d
                            }

        .mail-detail-content.disable-links a[href] {
            color: #aaa !important;
            text-decoration: line-through !important;
            cursor: default !important;
            pointer-events: none !important
        }

        .mail-detail-content .blockquote-toggle {
            color: #767676;
            font-size: 13px;
            padding-left: 56px;
            margin: 16px 0;
            min-height: 16px;
            word-break: break-word
        }

            .mail-detail-content .blockquote-toggle button.bqt {
                color: #696969;
                background-color: #eee;
                padding: 1px 10px;
                display: inline-block;
                font-size: 14px;
                line-height: 16px;
                cursor: pointer;
                outline: 0;
                position: absolute;
                left: 0;
                border: 0
            }

                .mail-detail-content .blockquote-toggle button.bqt:hover, .mail-detail-content .blockquote-toggle button.bqt:focus {
                    color: #fff;
                    background-color: #3c61aa;
                    text-decoration: none
                }

        .mail-detail-content .max-size-warning {
            color: #767676;
            padding: 16px 16px 0;
            border-top: 1px solid #ddd
        }

        .mail-detail-content a.deep-link {
            color: #fff;
            background-color: #3c61aa;
            text-decoration: none;
            font-size: 90%;
            font-weight: 700;
            font-family: -apple-system,BlinkMacSystemFont,"Helvetica Neue","Segoe UI",Arial,sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol" !important;
            padding: .1em 8px;
            border-radius: 3px
        }

            .mail-detail-content a.deep-link:hover, .mail-detail-content a.deep-link:focus, .mail-detail-content a.deep-link:active {
                color: #fff;
                background-color: #2f4b84
            }

        @media print {
            .mail-detail-content .collapsed-blockquote {
                display: block !important
            }

            .mail-detail-content .blockquote-toggle {
                display: none !important
            }
        }

        .mail-detail-content > div[id*=ox-] > h1, .mail-detail-content > div[id*=ox-] > h2, .mail-detail-content > div[id*=ox-] > h3, .mail-detail-content > div[id*=ox-] > h4, .mail-detail-content > div[id*=ox-] > h5 {
            margin-top: 0
        }
    </style>
</head>
<body class="mail-detail-content noI18n colorQuoted" role="complementary" aria-label="Purchase Order No.0017 Dt. 25-Mar-2022">
    <div style="word-spacing: 0px; font: 12px arial, sans-serif; text-transform: none; color: rgb(34,34,34); text-indent: 0px; white-space: normal; letter-spacing: normal; background-color: rgb(255,255,255); text-align: center;">
        <br>
        <br style="font-size: medium; color: rgb(0,0,0); font-family: 'Times New Roman'">
        <table style="border-right: rgb(15,17,15) 1px solid; border-top: rgb(15,17,15) 1px solid; font-size: 8pt; border-left: rgb(15,17,15) 1px solid; width: 700px; color: rgb(0,0,0); border-bottom: rgb(15,17,15) 1px solid; font-family: Verdana; border-collapse: collapse; text-align: center;">

            <tbody>
                <tr>
                    <td style="font-size: 24pt; text-align: center; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px" bgcolor="#A4CBF0">
                        <strong>ENDEAVOUR AUTOMATION</strong>
                    </td>
                </tr>
                <tr></tr>
                <tr>
                    <td style="text-align: center; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; font-family: verdana; font-size: 9pt">
                        <strong></strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px" bgcolor="#A4CBF0">
                        Survey No. 27, Nimbalkar Nagar, Near Raghunandan Mangal Karyalay, Tathawade, Pune-411033
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                        Mob: 9860502108
                        Email: <a href="mailto:endeavour.automations@gmail.com" class="mailto-link" target="_blank">endeavour.automations@gmail.com</a>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; height: 24px; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-decoration: underline; font-family: verdana; font-size: 10pt" bgcolor="#A4CBF0">
                        <strong>PURCHASE ORDER</strong>
                    </td>
                </tr>
                <tr style="font-size: 8pt">
                    <td style="border-right: #000055 1px solid; padding-right: 5px; border-top: #000055 1px solid; padding-left: 5px; padding-bottom: 5px; border-left: #000055 1px solid; color: #000055; padding-top: 5px; border-bottom: #000055 1px solid; border-collapse: collapse; height: 20px; text-align: left">
                        <table style="width: 100%; font-family: Tahoma; font-size: 8pt; border: 1px solid #000055; border-collapse: collapse">
                            <tbody>
                                <tr>
                                    <td style="border-right: #000055 1px solid; padding-right: 5px; border-top: #000055 1px solid; padding-left: 5px; padding-bottom: 5px; border-left: #000055 1px solid; color: #000055; padding-top: 5px; border-bottom: #000055 1px solid; font-family: Tahoma; border-collapse: collapse; height: 24px; text-align: left" bgcolor="#A4CBF0" colspan="2">
                                        <strong><span>To</span></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 50%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;">
                                        <strong>BRISK ELECTRONICS PVT LTD.</strong><br>
                                        House No.607, GAT No. 182<br>Pune - Paud Road<br>At Post - Bhukum,<br>Mulshi<br>PUNE - 412115<br>Maharashtra
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 50%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px; vertical-align: top;">
                                        <span style="text-decoration: underline"></span>GST No. : 27AABCB4166A1Z5<br>State Code : 27 Maharashtra
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; font-family: Verdana; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                        <table style="width: 100%; font-family: Tahoma; font-size: 8pt; border: 1px solid #000055; border-collapse: collapse">
                            <tbody>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 15%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" bgcolor="#A4CBF0">
                                        <strong>Order No.</strong>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 33%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;">
                                        <strong>0017</strong>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 15%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" bgcolor="#A4CBF0">
                                        <strong>Order Date:</strong>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 33%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;">
                                        <strong>25/03/2022</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 15%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" bgcolor="#A4CBF0">
                                        <strong>Ref. No.</strong>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 33%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;">
                                        <strong>Mail</strong>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 15%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" bgcolor="#A4CBF0">
                                        <strong>Ref. Date:</strong>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 33%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;">
                                        <strong>25/03/2022</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 15%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" bgcolor="#A4CBF0">
                                        <strong>Credit Days</strong>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" colspan="3">
                                        <strong>0 Days</strong>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr style="font-size: 8pt">
                    <td style="text-align: left; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; font-family: verdana; font-size: 10pt" bgcolor="#A4CBF0">
                        <span style="font-size: 9pt"><strong></strong></span>
                    </td>
                </tr>
                <tr style="font-size: 8pt">
                    <td style="text-align: left; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; font-family: verdana; font-size: 10pt" bgcolor="#A4CBF0">
                        <span style="font-size: 9pt"><strong>Kind Attn : </strong></span>
                    </td>
                </tr>
                <tr>
                    <td style="border: 1px solid #000055; border-collapse: collapse; padding: 5px"></td>
                </tr>
                <tr>
                    <td style="border: 1px solid #000055; border-collapse: collapse; padding: 5px"></td>
                </tr>
                <tr>
                    <td style="text-align: center; height: 24px; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-decoration: underline; font-family: 		verdana; font-size: 8pt" bgcolor="#A4CBF0">
                        <strong>
                            PRODUCT DETAILS
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td style="border: 1px solid #000055; border-collapse: collapse; padding: 5px"></td>
                </tr>
                <tr>
                    <td style="border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                        <table style="width: 100%; font-family: Tahoma; font-size: 8pt; border: 1px solid #000055; border-collapse: collapse">
                            <tbody>
                                <tr>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: left; font-family: Tahoma; font-size: 8pt; width: 53%;" bgcolor="#A4CBF0">
                                        <strong>
                                            Product
                                            Description
                                        </strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: left; font-family: Tahoma; font-size: 8pt; width: 11%;" bgcolor="#A4CBF0">
                                        <strong>HSN/SAC Code</strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 6%;" bgcolor="#A4CBF0">
                                        <strong>
                                            Tax
                                            %
                                        </strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 7%;" bgcolor="#A4CBF0">
                                        <strong>Quantity</strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: left; font-family: Tahoma; font-size: 8pt; width: 5%;" bgcolor="#A4CBF0">
                                        <strong>Units</strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 8%;" bgcolor="#A4CBF0">
                                        <strong>Rate</strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 10%;" bgcolor="#A4CBF0">
                                        <strong>Amount</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 53%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                       $Description$<br><br>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 11%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                       $HSNSAC$
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 6%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                       $TAX$
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 7%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        $Quantity$
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 5%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                      $Units$
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 8%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        $Rate$
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 10%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        $Discount$
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 53%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        SMF VRLA Batteries 18Ah 12V for 2kVA UPS-01 Set<br>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 11%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        8507
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 6%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        28.00
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 7%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        1
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 5%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        NOS
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 8%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        11100.00
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 10%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        11100.00
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 53%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        RACKS &amp; LINKS for 2kVA UPS<br><br>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 11%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        8504
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 6%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        18.00
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 7%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        1
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 5%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        Pcs
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 8%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        1900.00
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 10%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        1900.00
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 53%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        RCCB
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 11%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        8504
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 6%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        18.00
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 7%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        1
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 5%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        NOS
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 8%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        1800.00
                                    </td>
                                    <td style="text-align: right; font-family: Tahoma; color: #000055; width: 10%; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                                        1800.00
                                    </td>
                                </tr>
                                <tr>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: left; font-family: Tahoma; font-size: 8pt; width: 5%;" bgcolor="#A4CBF0" colspan="5">
                                        <strong></strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 8%;" bgcolor="#A4CBF0">
                                        <strong>Total</strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 10%;" bgcolor="#A4CBF0">
                                        <strong>48800.00</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 8%;" colspan="6">
                                        <strong>C-Gst 9%</strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 10%;">
                                        <strong>3393.00</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 8%;" colspan="6">
                                        <strong>S-Gst 9%</strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 10%;">
                                        <strong>3393.00</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 8%;" colspan="6">
                                        <strong>C-Gst 14%</strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 10%;">
                                        <strong>1554.00</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 8%;" colspan="6">
                                        <strong>S-Gst 14%</strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 10%;">
                                        <strong>1554.00</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: left; font-family: Tahoma; font-size: 8pt; width: 5%;" bgcolor="#A4CBF0" colspan="5">
                                        <strong>Amount In Words :  Fifty Eight Thousand Six Hundred Ninety Four And Zero Paise Only.</strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 8%;" bgcolor="#A4CBF0">
                                        <strong>Grand Total</strong>
                                    </td>
                                    <td style="color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; text-align: right; font-family: Tahoma; font-size: 8pt; width: 10%;" bgcolor="#A4CBF0">
                                        <strong>58694.00</strong>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr style="font-size: 8pt">
                    <td style="text-align: left; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; font-family: verdana; font-size: 10pt" bgcolor="#A4CBF0">
                        <span style="font-size: 7pt"><strong>" Subject To Pune Jurisdiction Only "</strong></span>
                    </td>
                </tr>
                <tr>
                    <td style="border-right: #000055 1px solid; padding-right: 5px; border-top: #000055 1px solid; padding-left: 5px; padding-bottom: 5px; border-left: #000055 1px solid; padding-top: 5px; border-bottom: #000055 1px solid; border-collapse: collapse"></td>
                </tr>
                <tr style="font-size: 8pt">
                    <td style="text-align: left; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; font-family: verdana; font-size: 10pt" bgcolor="#A4CBF0">
                        <span style="font-size: 8pt"><strong>Terms &amp; Conditions : </strong></span>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; font-family: Verdana; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px">
                        <table style="width: 100%; font-family: Tahoma; font-size: 8pt; border: 1px solid #000055; border-collapse: collapse">
                            <tbody>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 15%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" bgcolor="#A4CBF0">
                                        <strong>Taxes</strong>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" colspan="3">
                                        <strong>As Applicable</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 15%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" bgcolor="#A4CBF0">
                                        <strong>Warranty</strong>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" colspan="3">
                                        <strong>For UPS  02 Yrs</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 15%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" bgcolor="#A4CBF0">
                                        <strong>Warranty</strong>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" colspan="3">
                                        <strong>For Batteries 01 Yr</strong>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr style="font-size: 8pt">
                    <td style="border-right: #000055 1px solid; padding-right: 5px; border-top: #000055 1px solid; padding-left: 5px; padding-bottom: 5px; border-left: #000055 1px solid; padding-top: 5px; border-bottom: #000055 1px solid; border-collapse: collapse">
                        <table style="width: 100%; font-family: Tahoma; font-size: 8pt; border: 1px solid #000055; border-collapse: collapse">
                            <tbody>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 50%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" bgcolor="#A4CBF0">
                                        <strong>GST. TIN NO. : 27AFYPJ3488G1ZQ W.E.F. : 01-07-2017<br>State Code : 27 Maharashtra</strong>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 50%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;">
                                        <strong>
                                            For ENDEAVOUR AUTOMATION<br><br><br>Authorised Signatory<br>
                                            <span style="font-size: 7pt; color: #006600">
                                                (Computer generated document no signature required)
                                            </span>
                                        </strong>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; border-right: #000055 1px solid; padding-right: 5px; border-top: #000055 1px solid; padding-left: 5px; padding-bottom: 5px; border-left: #000055 1px solid; padding-top: 5px; border-bottom: #000055 1px solid; border-collapse: collapse; height: 10px">
                        <table style="font-family: Tahoma; font-size: 8pt; border: 1px solid #000055; border-collapse: collapse; width: 100%;">
                            <tbody>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 50%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;" bgcolor="#A4CBF0">
                                        <strong><span style="color: #ff0000">Please Find Attached :</span></strong>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; color: #000055; width: 50%; border: 1px solid #000055; border-collapse: collapse; padding: 5px; height: 24px;">
                                        <strong><span style="font-size: 7pt; color: #006600"><span style="font-size: 8pt; color: #000055">1. Pdf Copy<br>2. Word/Html Copy</span></span></strong>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <br>
    </div>
    <br><div style="word-spacing: 0px; font: 12px arial, sans-serif; text-transform: none; color: rgb(34,34,34); text-indent: 0px; white-space: normal; letter-spacing: normal; background-color: rgb(255,255,255); orphans: auto; widows: 1; webkit-text-stroke-width: 0px">
        <br>
        <br>
        <span style="font-size: 10pt; color: rgb(0,153,0)">
            <strong>
                Please consider the environment
                before printing this e-mail (Go Green)
            </strong>
        </span><br>
        <br>
        <a target="_blank" href="http://orchidtechno.in/NetHelp/default.htm?turl=Documents%2FSalient_Features.htm" rel="noopener">
            <span style="font-family: Tahoma">
                Auto Generated
                From <strong><em>Mark Server E.R.P.</em></strong>
                System)

                <br>
                <span style="font-family: Tahoma">http://www.orchidtechno.in</span>
            </span>
        </a><span style="font-family: Tahoma">&nbsp;</span><br>
        <br>
        <br>
        <br>
    </div>
</body>
</html>
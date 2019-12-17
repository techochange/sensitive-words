using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BMT.Server.Infrastructure
{
    [XmlRoot("xml")]
    public class WechatOrder
    {
        /// <summary>
        /// 小程序Id
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 签名类型
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 标价金额，单位是分
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 异步通知地址
        /// </summary>
        public string notify_url { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; } = "JSAPI";
        /// <summary>
        /// 用户标识
        /// </summary>
        public string openid { get; set; }
    }
    [XmlRoot("xml")]
    public class WechatOrderResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int err_code_des { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string prepay_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string code_url { get; set; }
    }
    public class WechatPaySignDto
    {
        /// <summary>
        /// 小程序id
        /// </summary>
        public string appId { get; set; }
        /// <summary>
        /// 时间戳（秒）
        /// </summary>
        public string timeStamp { get; set; }
        /// <summary>
        /// 随机串
        /// </summary>
        public string nonceStr { get; set; }
        /// <summary>
        /// prepay_id=xxx
        /// </summary>
        public string package { get; set; }
        /// <summary>
        /// 签名类型
        /// </summary>
        public string signType { get; set; }
        /// <summary>
        /// 支付签名
        /// </summary>
        public string paySign { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public string orderId { get; set; }

        public bool Success { get; set; }
    }

    [XmlRoot("xml")]
    public class WechatNotifyDto
    {

        ///<summary> 
        /// 小程序ID 
        /// 微信分配的小程序ID 
        /// </summary> 
        public string appid { get; set; }
        ///<summary> 
        /// 商户号 
        /// 微信支付分配的商户号 
        /// </summary> 
        public string mch_id { get; set; }
        ///<summary> 
        /// 设备号 
        /// 微信支付分配的终端设备号， 
        /// </summary> 
        public string device_info { get; set; }
        ///<summary> 
        /// 随机字符串 
        /// 随机字符串，不长于32位 
        /// </summary> 
        public string nonce_str { get; set; }
        ///<summary> 
        /// 签名 
        /// 签名，详见签名算法 
        /// </summary> 
        public string sign { get; set; }
        ///<summary> 
        /// 签名类型 
        /// 签名类型，目前支持HMAC-SHA256和MD5，默认为MD5 
        /// </summary> 
        public string sign_type { get; set; }
        ///<summary> 
        /// 业务结果 
        /// SUCCESS/FAIL 
        /// </summary> 
        public string result_code { get; set; }
        ///<summary> 
        /// 错误代码 
        /// 错误返回的信息描述 
        /// </summary> 
        public string err_code { get; set; }
        ///<summary> 
        /// 错误代码描述 
        /// 错误返回的信息描述 
        /// </summary> 
        public string err_code_des { get; set; }
        ///<summary> 
        /// 用户标识 
        /// 用户在商户appid下的唯一标识 
        /// </summary> 
        public string openid { get; set; }
        ///<summary> 
        /// 是否关注公众账号 
        /// 用户是否关注公众账号，Y-关注，N-未关注 
        /// </summary> 
        public string is_subscribe { get; set; }
        ///<summary> 
        /// 交易类型 
        /// JSAPI、NATIVE、APP 
        /// </summary> 
        public string trade_type { get; set; }
        ///<summary> 
        /// 付款银行 
        /// 银行类型，采用字符串类型的银行标识，银行类型见银行列表 
        /// </summary> 
        public string bank_type { get; set; }
        ///<summary> 
        /// 订单金额 
        /// 订单总金额，单位为分 
        /// </summary> 
        public int total_fee { get; set; }
        ///<summary> 
        /// 应结订单金额 
        /// 应结订单金额=订单金额-非充值代金券金额，应结订单金额<=订单金额。 
        /// </summary> 
        public int settlement_total_fee { get; set; }
        ///<summary> 
        /// 货币种类 
        /// 货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型 
        /// </summary> 
        public string fee_type { get; set; }
        ///<summary> 
        /// 现金支付金额 
        /// 现金支付金额订单现金支付金额，详见支付金额 
        /// </summary> 
        public int cash_fee { get; set; }
        ///<summary> 
        /// 现金支付货币类型 
        /// 货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型 
        /// </summary> 
        public string cash_fee_type { get; set; }
        ///<summary> 
        /// 总代金券金额 
        /// 代金券金额  小于=订单金额，订单金额-代金券金额=现金支付金额，详见支付金额 
        /// </summary> 
        public int coupon_fee { get; set; }
        ///<summary> 
        /// 代金券使用数量 
        /// 代金券使用数量 
        /// 并且订单使用了免充值券后有返回（取值：CASH、NO_CASH）。$n为下标,从0开始编号，举例：coupon_type_0 
        /// </summary> 
        public int coupon_count { get; set; }
        ///<summary> 
        /// 代金券类型 
        /// CASH--充值代金券  
        /// NO_CASH---非充值代金券 
        /// </summary> 
        [JsonProperty("coupon_type_$n")]
        public string coupon_type_n { get; set; }

        ///<summary> 
        /// 代金券ID 
        /// 代金券ID,$n为下标，从0开始编号 
        /// </summary> 
        [JsonProperty("coupon_id_$n")]
        public string coupon_id_n { get; set; }
        ///<summary> 
        /// 单个代金券支付金额 
        /// 单个代金券支付金额,$n为下标，从0开始编号 
        /// </summary> 
        [JsonProperty("coupon_fee_$n")]
        public int coupon_fee_n { get; set; }
        ///<summary> 
        /// 微信支付订单号 
        /// 微信支付订单号 
        /// </summary> 
        public string transaction_id { get; set; }
        ///<summary> 
        /// 商户订单号 
        /// 商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@ ，且在同一个商户号下唯一。 
        /// </summary> 
        public string out_trade_no { get; set; }
        ///<summary> 
        /// 商家数据包 
        /// 商家数据包，原样返回 
        /// </summary> 
        public string attach { get; set; }
        ///<summary> 
        /// 支付完成时间 
        /// 支付完成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则 
        /// </summary> 
        public string time_end { get; set; }

    }


    public class WechatNotifyResponse
    {
        public string return_code { get; set; }

        public string return_msg { get; set; }
    }
    public class WechatPayHelper
    {
        public static async Task<WechatOrderResult> DoMakeOrder(WechatOrder order)
        {
            var result = await HttpClientHelper.PostResponseJson("https://api.mch.weixin.qq.com/pay/unifiedorder", XmlHelper.Serialize(order));
            if (result.Success)
            {
                return XmlHelper.Deserialize<WechatOrderResult>(result.Content);
            }
            return null;
        }


        public static string GetOrderSign(WechatOrder order,string key)
        {
            var str = $"appid={order.appid}&body={order.body}&device_info={order.device_info}&mch_id={order.mch_id}&nonce_str={order.nonce_str}&notify_url={order.notify_url}&openid={order.openid}&out_trade_no={order.out_trade_no}&sign_type={order.sign_type}&spbill_create_ip={order.spbill_create_ip}&total_fee={order.total_fee}&trade_type={order.trade_type}";
            if (string.IsNullOrEmpty(key))
            {
                key = "ABCabc1234ABCabc1234ABCabc1234AB";
            }
            var strA = str + "&key="+key;
            var md5 = SecurityHelper.ToMD5(strA);
            return md5;
        }


        public static string GetPaySign(WechatPaySignDto dto, string key)
        {
            var str = $"appId={dto.appId}&nonceStr={dto.nonceStr}&package={dto.package}&signType={dto.signType}&timeStamp={dto.timeStamp}";
            if (string.IsNullOrEmpty(key))
            {
                key = "ABCabc1234ABCabc1234ABCabc1234AB";
            }
            var strA = str + "&key=" + key;
            var md5 = SecurityHelper.ToMD5(strA);
            return md5;
        }
    }
}

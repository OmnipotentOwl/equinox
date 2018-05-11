module Foldunk.UnionCodec

open Newtonsoft.Json
open TypeShape

/// Newtonsoft.Json implementation of IEncoder that encodes direct to a UTF-8 Buffer
type JsonUtf8Encoder(settings : JsonSerializerSettings) =
    interface UnionEncoder.IEncoder<byte[]> with
        member __.Encode (value : 'T) =
            JsonConvert.SerializeObject(value, settings)
            |> System.Text.Encoding.UTF8.GetBytes
        member __.Decode (json : byte[]) =
            let x = System.Text.Encoding.UTF8.GetString(json)
            JsonConvert.DeserializeObject<'T>(x, settings)

/// Generates an event sum encoder using Newtonsoft.Json for individual event types that serializes to a UTF-8 array buffer
let generateJsonUtf8UnionCodec<'Union>(settings) =
    UnionEncoder.UnionEncoder<'Union>.Create(new JsonUtf8Encoder(settings))
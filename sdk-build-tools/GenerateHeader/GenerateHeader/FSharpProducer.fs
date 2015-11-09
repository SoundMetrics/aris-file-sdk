module FSharpProducer

open CodeProducer

type FSharpProducer() =

    interface ICodeProducer with

        member __.WriteTypeComments (basics : ProducerBasics) (typ : TypeInfo) = () // TODO
        member __.WriteTypePreface (basics : ProducerBasics) (typ : TypeInfo) = () // TODO
        member __.WriteTypeBegin (basics : ProducerBasics) (typ : TypeInfo) = () // TODO
        member __.WriteTypeEnd (basics : ProducerBasics) (typ : TypeInfo) = () // TODO

        member __.WritFieldComments (basics : ProducerBasics) (field : FieldInfo) = () // TODO
        member __.WritFieldPreface (basics : ProducerBasics) (field : FieldInfo) = () // TODO
        member __.WritField (basics : ProducerBasics) (field : FieldInfo) = () // TODO

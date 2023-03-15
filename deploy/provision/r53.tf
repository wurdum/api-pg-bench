resource "aws_route53_zone" "local" {
  name = var.domain_name

  vpc {
    vpc_id = aws_vpc.vpc.id
  }
}

resource "aws_route53_record" "k6" {
  zone_id = aws_route53_zone.local.zone_id
  name    = "k6.${var.domain_name}"
  type    = "A"
  ttl     = "300"
  records = [
    aws_instance.k6.private_ip
  ]
}

resource "aws_route53_record" "db" {
  zone_id = aws_route53_zone.local.zone_id
  name    = "db.${var.domain_name}"
  type    = "A"
  ttl     = "300"
  records = [
    aws_instance.db.private_ip
  ]
}

resource "aws_route53_record" "api" {
  zone_id = aws_route53_zone.local.zone_id
  name    = "api.${var.domain_name}"
  type    = "A"
  ttl     = "300"
  records = [
    aws_instance.api.private_ip
  ]
}

resource "aws_route53_record" "monitor" {
  zone_id = aws_route53_zone.local.zone_id
  name    = "monitor.${var.domain_name}"
  type    = "A"
  ttl     = "300"
  records = [
    aws_instance.monitor.private_ip
  ]
}

output "k6_r53_dns_name" {
  value = aws_route53_record.k6.name
}

output "db_r53_dns_name" {
  value = aws_route53_record.db.name
}

output "api_r53_dns_name" {
  value = aws_route53_record.api.name
}

output "monitor_r53_dns_name" {
  value = aws_route53_record.monitor.name
}
